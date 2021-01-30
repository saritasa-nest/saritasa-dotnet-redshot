using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Download form.
    /// Shows downloading progress.
    /// </summary>
    public class DownloadForm : Form
    {
        private ProgressBar progressBar;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="downloader">Downloader.</param>
        /// <param name="downloadTitle">Download title.</param>
        public DownloadForm(Downloader downloader, string downloadTitle)
        {
            Title = downloadTitle;
            Icon = new Icon(1, Icons.RedCircle);
            downloader.WebClient.DownloadFileCompleted += DownloaderDownloadFileCompleted;
            downloader.WebClient.DownloadProgressChanged += DownloaderDownloadProgressChanged;
            Shown += DownloadFormShown;

            Maximizable = false;
            Resizable = false;

            IntitializeComponents();
        }

        private void DownloadFormShown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void DownloaderDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void DownloaderDownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            progressBar.Value = 100;
            Close();
        }

        private void IntitializeComponents()
        {
            progressBar = new ProgressBar()
            {
                Width = 350,
                Height = 30,
                MinValue = 0,
                MaxValue = 100
            };

            Padding = 20;
            Size = new Size(390, 100);

            Content = progressBar;
        }
    }
}
