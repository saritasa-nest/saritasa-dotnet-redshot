using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RedShot.Helpers.CLI
{
    public class CliManager : IDisposable
    {
        public event DataReceivedEventHandler OutputDataReceived;

        public event DataReceivedEventHandler ErrorDataReceived;

        public bool IsProcessRunning { get; private set; }

        private Process process;

        public int Open(string path, string args = null)
        {
            if (File.Exists(path))
            {
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo()
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
                    if (psi.RedirectStandardOutput)
                    {
                        process.OutputDataReceived += CliOutputDataReceived;
                    }

                    if (psi.RedirectStandardError)
                    {
                        process.ErrorDataReceived += CliErrorDataReceived;
                    }

                    process.StartInfo = psi;

                    process.Start();

                    if (psi.RedirectStandardOutput)
                    {
                        process.BeginOutputReadLine();
                    }

                    if (psi.RedirectStandardError)
                    {
                        process.BeginErrorReadLine();
                    }

                    try
                    {
                        IsProcessRunning = true;
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

        private void CliOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (OutputDataReceived != null)
                {
                    OutputDataReceived(sender, e);
                }
            }
        }

        private void CliErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (ErrorDataReceived != null)
                {
                    ErrorDataReceived(sender, e);
                }
            }
        }

        public void WriteInput(string input)
        {
            if (IsProcessRunning && process != null && process.StartInfo != null && process.StartInfo.RedirectStandardInput)
            {
                process.StandardInput.WriteLine(input);
            }
        }

        public void Close()
        {
            if (IsProcessRunning && process != null)
            {
                WriteInput("q");
                process.CloseMainWindow();
                process.Kill();
            }
        }

        public void Dispose()
        {
            if (process != null)
            {
                process.Dispose();
            }
        }
    }
}
