using System.Diagnostics;
using System.Text;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// CLI manager.
    /// </summary>
    public class CliManager
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetLogger("CLIdebug");

        /// <summary>
        /// File path of the process's binary.
        /// </summary>
        protected readonly string filePath;

        /// <summary>
        /// Process state.
        /// </summary>
        public bool IsProcessRunning { get; protected set; }

        /// <summary>
        /// Output from CLI execution.
        /// </summary>
        public StringBuilder Output { get; protected set; }

        /// <summary>
        /// Process field.
        /// </summary>
        protected Process process;

        /// <summary>
        /// Initializes CLI manager.
        /// </summary>
        public CliManager(string filePath)
        {
            this.filePath = filePath;
            Output = new StringBuilder();
        }

        /// <summary>
        /// Run process.
        /// </summary>
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

        /// <summary>
        /// Data received handler.
        /// </summary>
        protected virtual void DataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            logger.Trace(data);

            if (!string.IsNullOrEmpty(data))
            {
                Output.AppendLine(data);
            }
        }

        /// <summary>
        /// Input message to CLI.
        /// </summary>
        public virtual void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                process?.StandardInput.WriteLine(input);
            }
        }

        /// <summary>
        /// Stop process.
        /// </summary>
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
