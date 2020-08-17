using System.Diagnostics;
using System.Text;

namespace RedShot.Helpers.CLI
{
    public class CliManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("CLIdebug");

        public bool IsProcessRunning { get; private set; }

        public StringBuilder Output { get; private set; }

        private Process process;

        public CliManager()
        {
            Output = new StringBuilder();
        }

        public void Run(string args)
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
                    Arguments = args,
                    UseShellExecute = true,
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

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            Logger.Trace(data);

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

        public void Stop()
        {
            if (IsProcessRunning)
            {
                process?.Close();
                process?.Kill();
            }
        }
    }
}
