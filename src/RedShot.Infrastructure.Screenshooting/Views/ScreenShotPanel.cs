using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Resources;

namespace RedShot.Infrastructure.Screenshooting.Views
{
    /// <summary>
    /// Screen shot panel.
    /// </summary>
    internal class ScreenShotPanel : Form
    {
        /// <summary>
        /// Button for start uploading.
        /// </summary>
        public ImageButton FinishSelectionButton { get; }

        /// <summary>
        /// Initializes selection panel view.
        /// </summary>
        public ScreenShotPanel()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(84, 54);

            var buttonSize = new Size(80, 50);
            var imageSize = new Size(40, 40);

            var uploadImage = Icons.Upload;

            FinishSelectionButton = new ImageButton(buttonSize, uploadImage, scaleImageSize: imageSize)
            {
                ToolTip = "Upload screenshot"
            };

            Content = new StackLayout
            {
                Padding = 2,
                Orientation = Orientation.Horizontal,
                Items =
                {
                    FinishSelectionButton,
                }
            };
        }
    }
}
