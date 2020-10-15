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
        private Label stateInfo;

        /// <summary>
        /// Initializes download form.
        /// </summary>
        public DownloadForm(Downloader downloader, string title)
        {
            Icon = new Icon(1, Icons.RedCircle);
            Title = title;
            downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;
            downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
            this.Shown += DownloadForm_Shown;

            IntitializeComponents();
        }

        private void DownloadForm_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void Downloader_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            stateInfo.Text = $"Kilobytes received: {e.BytesReceived / 1024} Kb / {e.TotalBytesToReceive/1024} Kb";
        }

        private void Downloader_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            progressBar.Value = 100;
            stateInfo.Text = "Finished!";
            Task.Delay(1000).Wait();
            Close();
        }

        private void IntitializeComponents()
        {
            progressBar = new ProgressBar()
            {
                Width = 250,
                Height = 20,
                MinValue = 0,
                MaxValue = 100
            };

            stateInfo = new Label()
            {
                Height = 20
            };

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    progressBar,
                    stateInfo
                }
            };
        }
    }
}
