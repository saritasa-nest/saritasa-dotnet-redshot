using Eto.Forms;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Common.Forms
{
    public class DownloadForm : Form
    {
        private ProgressBar progressBar;
        private Label stateInfo;

        public DownloadForm(Downloader downloader, string title)
        {
            Title = title;
            downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;
            downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;

            IntitializeComponents();

            this.Shown += DownloadForm_Shown;
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
                Items =
                {
                    FormsHelper.VoidBox(10),
                    progressBar,
                    FormsHelper.VoidBox(10),
                    stateInfo
                }
            };
        }
    }
}
