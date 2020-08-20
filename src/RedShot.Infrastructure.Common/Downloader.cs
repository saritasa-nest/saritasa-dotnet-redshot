using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Common
{
    public class Downloader : IDisposable
    {
        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

        public event EventHandler<AsyncCompletedEventArgs> DownloadFileCompleted;

        private readonly string downloadDirectory;
        private WebClient webClient;
        private bool disposed;

        public Downloader()
        {
            webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

            var tempDirectory = Path.GetTempPath();
            downloadDirectory = Directory.CreateDirectory(Path.Combine(tempDirectory, Guid.NewGuid().ToString())).FullName;
        }

        private void RunForm(string fileName)
        {
            var form = new DownloadForm(this, fileName);
            form.Show();
        }

        public void DownloadAsync(string url, string fileName, Action<string> callback)
        {
            RunForm(fileName);

            var path = Path.Combine(downloadDirectory, fileName);

            webClient.DownloadFileTaskAsync(url, path);
            webClient.DownloadFileCompleted += (o, e) => callback.Invoke(path);
        }

        public string Download(string url, string fileName)
        {
            var path = Path.Combine(downloadDirectory, fileName);

            webClient.DownloadFile(url, path);

            return path;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadFileCompleted?.Invoke(sender, e);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged?.Invoke(sender, e);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                webClient.Dispose();
                webClient = null;

                disposed = true;
            }
        }
    }
}
