using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Helpers.Ffmpeg
{
    public class FFmpegCliManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("Ffmpeg");
        private bool isLinuxOs = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public event DataReceivedEventHandler OutputDataReceived;

        public event DataReceivedEventHandler ErrorDataReceived;

        public bool IsProcessRunning { get; private set; }

        public string FFmpegPath { get; private set; }

        public StringBuilder Output { get; private set; }

        public bool IsProcessFinished { get; private set; }

        private Process process;

        public FFmpegCliManager(string ffmpegPath = null)
        {
            FFmpegPath = ffmpegPath;
            Output = new StringBuilder();
            OutputDataReceived += FFmpeg_DataReceived;
            ErrorDataReceived += FFmpeg_DataReceived;
        }

        public void Run(string args)
        {
            Output.Clear();

            if (IsProcessRunning)
            {
                Stop();
            }

            Open(args, FFmpegPath);
        }

        public void Open(string args, string path = null)
        {
            if (File.Exists(path))
            {
                IsProcessFinished = false;

                Task.Run(() =>
                {
                    using (process = new Process())
                    {
                        var processInfo = new ProcessStartInfo()
                        {
                            Arguments = args,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            StandardOutputEncoding = Encoding.UTF8,
                            StandardErrorEncoding = Encoding.UTF8
                        };

                        if (isLinuxOs)
                        {
                            processInfo.UseShellExecute = true;
                        }
                        else
                        {
                            processInfo.FileName = path;
                            processInfo.WorkingDirectory = Path.GetDirectoryName(path);
                        }

                        process.EnableRaisingEvents = true;

                        process.OutputDataReceived += CliOutputDataReceived;
                        process.ErrorDataReceived += CliErrorDataReceived;

                        process.StartInfo = processInfo;

                        IsProcessRunning = true;
                        try
                        {
                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                            process.WaitForExit();
                        }
                        finally
                        {
                            IsProcessRunning = false;
                            IsProcessFinished = true;
                        }
                    }
                });
            }
        }

        public void WaitForExit()
        {
            while (!IsProcessFinished)
            {
            }
        }

        public void Stop()
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

                        Task.Delay(300).Wait();
                    }
                    else
                    {
                        return;
                    }
                }

                // If still running.
                if (IsProcessRunning)
                {
                    process.Kill();
                }
            }
        }

        private void CliOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.Trace(e.Data);
            OutputDataReceived?.Invoke(sender, e);
        }

        private void CliErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.Trace(e.Data);
            ErrorDataReceived?.Invoke(sender, e);
        }

        private void FFmpeg_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            if (!string.IsNullOrEmpty(data))
            {
                Output.AppendLine(data);
            }
        }

        public void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                process?.StandardInput.WriteLine(input);
            }
        }

        ~FFmpegCliManager()
        {
            Stop();
        }
    }
}
