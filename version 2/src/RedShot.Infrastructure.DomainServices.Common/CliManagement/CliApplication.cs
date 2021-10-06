using Saritasa.Tools.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.DomainServices.Common.CliManagement
{
    public class CliApplication : IDisposable
    {
        private ReplaySubject<string> responseMessages;
        private StringBuilder outputBuilder;
        private volatile bool hasStarted;
        private Process cliProcess;
        private bool disposedValue;
        private SemaphoreSlim applicationSemaphore;

        public string Output => outputBuilder.ToString();

        public IObservable<string> ResponseMessages => responseMessages;

        public CliApplication()
        {
            responseMessages = new ReplaySubject<string>();
            outputBuilder = new StringBuilder();
            applicationSemaphore = new SemaphoreSlim(0, 1);
        }

        public async Task StartAsync(string fileName, string arguments = "")
        {
            await applicationSemaphore.WaitAsync();

            if (hasStarted)
            {
                throw new DomainException("The CLI application is already started.");
            }

            hasStarted = true;

            var processInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            cliProcess = new Process()
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };

            cliProcess.OutputDataReceived += CliProcessOutputDataReceived;
            cliProcess.ErrorDataReceived += CliProcessErrorDataReceived;

            await Task.Run(() =>
            {
                cliProcess.Start();
            });

            applicationSemaphore.Release();
        }

        private void CliProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            responseMessages.OnNext(e.Data);
            outputBuilder.Append(e.Data);
        }

        private void CliProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            responseMessages.OnError(new DomainException("An error occurred in the CLI process", e.Data));
            outputBuilder.Append(e.Data);
        }

        public async Task InputCommandAsync(string command)
        {
            await applicationSemaphore.WaitAsync();

            if (hasStarted)
            {
                throw new DomainException("The CLI application should be started to input commands.");
            }

            await Task.Run(async () =>
            {
                cliProcess.WaitForInputIdle();
                await cliProcess.StandardInput.WriteLineAsync(command);
            });

            applicationSemaphore.Release();
        }

        public async Task CloseAsync()
        {
            await applicationSemaphore.WaitAsync();

            if (!hasStarted)
            {
                throw new DomainException("The CLI application has not started yet.");
            }

            cliProcess.CloseMainWindow();
            await KillProcessSafe();
            cliProcess.Close();
            responseMessages.OnCompleted();

            responseMessages = new ReplaySubject<string>();
            outputBuilder = new StringBuilder();

            hasStarted = false;

            applicationSemaphore.Release();
        }

        public async Task WaitForExitAsync()
        {
            await applicationSemaphore.WaitAsync();

            if (cliProcess is null)
            {
                throw new DomainException("The CLI application is not initialized yet.");
            }

            if (!hasStarted)
            {
                return;
            }

            await Task.Run(() =>
            {
                if (IsProcessRunning())
                {
                    cliProcess.WaitForExit();
                }
            });

            applicationSemaphore.Release();
        }

        private async Task KillProcessSafe()
        {
            try
            {
                do
                {
                    await Task.Delay(300);
                    cliProcess.Kill(entireProcessTree: true);
                }
                while (IsProcessRunning());
            }
            catch
            {
            }
        }

        private bool IsProcessRunning()
        {
            var processes = Process.GetProcesses();

            return processes
                .Where(p => !p.HasExited)
                .Any(p => p.Id == cliProcess.Id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    responseMessages.Dispose();
                    outputBuilder.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                if (hasStarted)
                {
                    cliProcess.CloseMainWindow();
                    cliProcess.Close();
                }
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~CliApplication()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
