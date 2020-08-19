using System.Diagnostics;
using System.Text;

namespace RedShot.Infrastructure.Common
{
    public class CliManager
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetLogger("CLIdebug");

        protected readonly string filePath;

        public bool IsProcessRunning { get; protected set; }

        public StringBuilder Output { get; protected set; }

        protected Process process;

        public CliManager(string filePath)
        {
            this.filePath = filePath;
            Output = new StringBuilder();
        }

        public virtual void Run(string args)
        {
            Output.Clear();

            if (IsProcessRunning)
            {
                Stop();
            }

            using (process = new Process())
            {
                var processInfo = new ProcessStartInfo()
                {
                    FileName = filePath,
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

                process.OutputDataReceived += DataReceived;
                process.ErrorDataReceived += DataReceived;

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
            }
        }

        protected virtual void DataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            logger.Trace(data);

            if (!string.IsNullOrEmpty(data))
            {
                Output.AppendLine(data);
            }
        }

        public virtual void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                process?.StandardInput.WriteLine(input);
            }
        }

        public virtual void Stop()
        {
            if (IsProcessRunning)
            {
                process?.Close();
                process?.Kill();
            }
        }
    }
}
