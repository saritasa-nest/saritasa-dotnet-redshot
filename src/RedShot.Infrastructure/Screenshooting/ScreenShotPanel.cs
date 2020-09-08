using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Screenshooting
{
    /// <summary>
    /// Painting panel for editor view.
    /// </summary>
    public class ScreenShotPanel : Form
    {
        /// <summary>
        /// Button for enabling painting mode.
        /// </summary>
        public ImageButton EnablePaintingModeButton { get; }

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

            var paintImage = new Bitmap(Resources.Properties.Resources.Paint);

            EnablePaintingModeButton = new ImageButton(buttonSize, paintImage, scaleImageSize: imageSize)
            {
                ToolTip = "Open image editor"
            };

            Content = new StackLayout
            {
                Padding = 2,
                Orientation = Orientation.Horizontal,
                Items =
                {
                    EnablePaintingModeButton,
                }
            };
        }
    }
}
