using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Recording.Settings
{
    /// <summary>
    /// Control for showing that FFmpeg is not installed.
    /// </summary>
    internal class FfmpegUninstalledControl : Panel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public FfmpegUninstalledControl()
        {
            var installButton = new Button()
            {
                Text = "Install",
                ToolTip = "Install",
                Size = new Size(80, 25)
            };
            installButton.Click += (o, e) => RecordingManager.Instance.RecordingService.InstallFFmpeg();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                Spacing = 10,
                Items =
                {
                    new Label()
                    {
                        Text = "FFmpeg is not installed",
                        TextColor = Colors.Red
                    },
                    installButton
                }
            };
        }
    }
}
