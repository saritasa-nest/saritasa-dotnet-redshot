using Eto.Forms;
using Eto.Drawing;
using RedShot.Eto.Desktop.Resources.Controls.Buttons;
using RedShot.Eto.Desktop.Resources;

namespace RedShot.Infrastructure.Screenshooting.Views
{
    /// <summary>
    /// Painting panel for painting selection view.
    /// </summary>
    internal class ScreenShotPanel : Form
    {
        /// <summary>
        /// Button for enabling painting mode.
        /// </summary>
        public ImageButton FinishSelectionButton { get; }

        /// <summary>
        /// Initializes painting panel view.
        /// </summary>
        public ScreenShotPanel()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(84, 54);

            var buttonSize = new Size(80, 50);
            var imageSize = new Size(40, 40);

            var paintImage = Icons.Paint;

            FinishSelectionButton = new ImageButton(buttonSize, paintImage, scaleImageSize: imageSize)
            {
                ToolTip = "Open image editor"
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
