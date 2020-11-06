using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Provides downloading data.
    /// </summary>
    public class Downloader : IDisposable
    {
        private readonly string downloadDirectory;
        private WebClient webClient;
        private bool disposed;
        private string title;

        /// <summary>
        /// Download progress changed event.
        /// </summary>
        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Download file completed event.
        /// </summary>
        public event EventHandler<AsyncCompletedEventArgs> DownloadFileCompleted;

        /// <summary>
        /// Initializes downloader.
        /// </summary>
        public Downloader(string title)
        {
            this.title = title;
            webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

            var tempDirectory = Path.GetTempPath();
            downloadDirectory = Directory.CreateDirectory(Path.Combine(tempDirectory, Guid.NewGuid().ToString())).FullName;
        }

        /// <summary>
        /// Download data asynchronously.
        /// </summary>
        /// <param name="callback">A delegate that will be invoked after data is downloaded.</param>
        public void DownloadAsync(string url, string fileName, Action<string> callback)
        {
            RunForm();
            var path = Path.Combine(downloadDirectory, fileName);

            webClient.DownloadFileAsync(new Uri(url), path);
            webClient.DownloadFileCompleted += (o, e) => callback.Invoke(path);
        }

        /// <summary>
        /// Download data.
        /// Blocking operation.
        /// </summary>
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

        private void RunForm()
        {
            var form = new DownloadForm(this)
            {
                Title = title
            };

            form.Show();
        }

        /// <summary>
        /// Disposes web client.
        /// </summary>
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
