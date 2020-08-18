using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RedShot.Helpers.Ffmpeg
{
    public sealed class FFmpegCliManager : CliManager
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetLogger("Ffmpeg");

        public event DataReceivedEventHandler OutputDataReceived;

        public event DataReceivedEventHandler ErrorDataReceived;

        public bool IsProcessFinished { get; private set; }

        public FFmpegCliManager(string ffmpegPath) : base(ffmpegPath)
        {
            OutputDataReceived += FFmpeg_DataReceived;
            ErrorDataReceived += FFmpeg_DataReceived;
        }

        public override void Run(string args)
        {
            Output.Clear();

            if (IsProcessRunning)
            {
                Stop();
            }

            Open(filePath, args);
        }

        public void Open(string path, string args)
        {
            IsProcessFinished = false;

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

        public void WaitForExit()
        {
            while (!IsProcessFinished)
            {
            }
        }

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

                //// If still running.
                //if (IsProcessRunning)
                //{
                //    process.Kill();
                //}
            }
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

        private void FFmpeg_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            if (!string.IsNullOrEmpty(data))
            {
                Output.AppendLine(data);
            }
        }

        ~FFmpegCliManager()
        {
            Stop();
        }
    }
}
