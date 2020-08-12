using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Recording.Helpers
{
    public class FFmpegCliManager
    {
        public event DataReceivedEventHandler OutputDataReceived;

        public event DataReceivedEventHandler ErrorDataReceived;

        public bool IsProcessRunning { get; private set; }

        public string FFmpegPath { get; private set; }

        public StringBuilder Output { get; private set; }

        public bool StopRequested { get; set; }

        private Process process;

        public FFmpegCliManager(string ffmpegPath)
        {
            FFmpegPath = ffmpegPath;
            Output = new StringBuilder();
            OutputDataReceived += FFmpeg_DataReceived;
            ErrorDataReceived += FFmpeg_DataReceived;
        }

        public bool Run(string args)
        {
            Output.Clear();

            if (IsProcessRunning)
            {
                Stop();
            }

            int errorCode = Open(FFmpegPath, args);
            return errorCode == 0;
        }

        public int Open(string path, string args = null)
        {
            if (File.Exists(path))
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
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
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
                    }

                    return process.ExitCode;
                }
            }

            return -1;
        }

        public void Stop()
        {
            if (IsProcessRunning && process != null)
            {
                int closeTryCount = 0;

                while (closeTryCount <= 3)
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

                process.Kill();
            }
        }

        private void CliOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(sender, e);
        }

        private void CliErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(sender, e);
        }

        private void FFmpeg_DataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (this)
            {
                string data = e.Data;

                if (!string.IsNullOrEmpty(data))
                {
                    Output.AppendLine(data);
                }
            }
        }

        public void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                process?.StandardInput.WriteLine(input);
            }
        }
    }
}
