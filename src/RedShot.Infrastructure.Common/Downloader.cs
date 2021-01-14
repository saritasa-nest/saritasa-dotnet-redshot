using System;
using System.IO;
using System.Net;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Provides downloading data.
    /// </summary>
    public class Downloader : IDisposable
    {
        private readonly string downloadDirectory;
        private bool disposed;

        /// <summary>
        /// Web client.
        /// </summary>
        public WebClient WebClient { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Downloader()
        {
            WebClient = new WebClient();
            var tempDirectory = Path.GetTempPath();
            downloadDirectory = Directory.CreateDirectory(Path.Combine(tempDirectory, Guid.NewGuid().ToString())).FullName;
        }

        /// <summary>
        /// Download data asynchronously.
        /// </summary>
        /// <param name="url">A URL to the file.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="callback">A delegate that will be invoked after data is downloaded, returns file path as a parameter.</param>
        public void DownloadAsync(string url, string fileName, Action<string> callback)
        {
            var path = Path.Combine(downloadDirectory, fileName);

            WebClient.DownloadFileAsync(new Uri(url), path);
            WebClient.DownloadFileCompleted += (o, e) =>
            {
                if (!e.Cancelled)
                {
                    callback.Invoke(path);
                }
            };
        }

        /// <summary>
        /// Download data.
        /// Blocking operation.
        /// </summary>
        public string Download(string url, string fileName)
        {
            var path = Path.Combine(downloadDirectory, fileName);

            WebClient.DownloadFile(url, path);

            return path;
        }

        /// <summary>
        /// Cancel downloading.
        /// </summary>
        public void CancelAsync()
        {
            WebClient.CancelAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!disposed)
            {
                WebClient.Dispose();
                disposed = true;
            }
        }
    }
}
