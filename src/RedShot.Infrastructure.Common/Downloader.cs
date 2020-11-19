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
        public Downloader()
        {
            webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClientDownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClientDownloadProgressChanged;

            var tempDirectory = Path.GetTempPath();
            downloadDirectory = Directory.CreateDirectory(Path.Combine(tempDirectory, Guid.NewGuid().ToString())).FullName;
        }

        /// <summary>
        /// Download data asynchronously.
        /// </summary>
        /// <param name="url">A URL to the file.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="callback">A delegate that will be invoked after data is downloaded, returns file path as a parameter.</param>
        /// <param name="downloadFormTitle">Download form title.</param>
        public void DownloadAsync(string url, string fileName, Action<string> callback, string downloadFormTitle = null)
        {
            if (string.IsNullOrWhiteSpace(downloadFormTitle))
            {
                downloadFormTitle = fileName;
            }

            RunForm(downloadFormTitle);
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

        private void WebClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadFileCompleted?.Invoke(sender, e);
        }

        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged?.Invoke(sender, e);
        }

        private void RunForm(string title)
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
