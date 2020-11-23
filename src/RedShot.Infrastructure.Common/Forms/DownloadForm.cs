using System.Threading.Tasks;
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
        /// Initializes download form.
        /// </summary>
        public DownloadForm(Downloader downloader)
        {
            Icon = new Icon(1, Icons.RedCircle);
            downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;
            downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
            this.Shown += DownloadForm_Shown;

            Maximizable = false;
            Resizable = false;

            IntitializeComponents();
        }

        private void DownloadForm_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void Downloader_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Downloader_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
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
