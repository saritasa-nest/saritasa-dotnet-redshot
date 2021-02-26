﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Recording.Common.Ffmpeg
{
    /// <summary>
    /// Provides functions to work with FFmpeg CLI.
    /// </summary>
    public sealed class FFmpegCliManager : CliManager
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetLogger("Ffmpeg");
        private readonly object lockObject = new object();

        private readonly Action<Process> setProcessTracker;

        /// <summary>
        /// Output data received event.
        /// </summary>
        public event DataReceivedEventHandler OutputDataReceived;

        /// <summary>
        /// Error data received event.
        /// </summary>
        public event DataReceivedEventHandler ErrorDataReceived;

        /// <summary>
        /// Recording status.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Initializes FFmpeg CLI manager.
        /// </summary>
        public FFmpegCliManager(string ffmpegPath, Action<Process> setProcessTracker = null) : base(ffmpegPath)
        {
            OutputDataReceived += FFmpegDataReceived;
            ErrorDataReceived += FFmpegDataReceived;
            AppDomain.CurrentDomain.ProcessExit += (o, e) => Stop();
            this.setProcessTracker = setProcessTracker;
        }

        /// <inheritdoc/>
        public override void Run(string args)
        {
            Output.Clear();

            if (IsProcessRunning)
            {
                Stop();
                WaitForExit();
            }

            Open(filePath, args);
        }

        /// <summary>
        /// Waits for the process finished.
        /// </summary>
        public void WaitForExit()
        {
            while (IsProcessRunning)
            {
                lock (lockObject)
                {
                    if (!IsProcessRunning)
                    {
                        break;
                    }
                }
                Task.Delay(100).Wait();
            }
        }

        /// <inheritdoc/>
        public override void Stop()
        {
            if (IsProcessRunning && process != null)
            {
                int closeTryCount = 0;

                while (closeTryCount <= 10)
                {
                    if (IsProcessRunning)
                    {
                        WriteInput("q");
                        closeTryCount++;

                        Task.Delay(500).Wait();
                    }
                    else
                    {
                        return;
                    }
                }

                process?.Kill();
            }
        }

        private void Open(string path, string args)
        {
            IsProcessRunning = true;

            Task.Run(() =>
            {
                using (process = new Process())
                {
                    var processInfo = new ProcessStartInfo()
                    {
                        FileName = path,
                        WorkingDirectory = Path.GetDirectoryName(path),
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8,
                        StandardErrorEncoding = System.Text.Encoding.UTF8
                    };

                    process.EnableRaisingEvents = true;
                    process.OutputDataReceived += CliOutputDataReceived;
                    process.ErrorDataReceived += CliErrorDataReceived;
                    process.OutputDataReceived += RecordingCheck;
                    process.ErrorDataReceived += RecordingCheck;
                    process.StartInfo = processInfo;

                    try
                    {
                        process.Start();
                        setProcessTracker?.Invoke(process);
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                    }
                    finally
                    {
                        process.Kill(true);
                        lock (lockObject)
                        {
                            IsProcessRunning = false;
                        }

                        IsRecording = false;
                        logger.Trace("Recording finished!");
                    }
                }
            });
        }

        private void CliOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            logger.Trace(e.Data);
            OutputDataReceived?.Invoke(sender, e);
        }

        private void CliErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            logger.Trace(e.Data);
            ErrorDataReceived?.Invoke(sender, e);
        }

        private void RecordingCheck(object sender, DataReceivedEventArgs e)
        {
            if (!IsRecording)
            {
                if (!string.IsNullOrEmpty(e.Data) && e.Data.Contains("Press [q] to stop"))
                {
                    IsRecording = true;
                    logger.Trace("Recording started!");
                }
            }
        }

        private void FFmpegDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            if (!string.IsNullOrEmpty(data))
            {
                Output.AppendLine(data);
            }
        }
    }
}
