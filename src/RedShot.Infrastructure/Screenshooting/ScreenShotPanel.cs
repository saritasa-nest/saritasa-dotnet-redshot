using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Screenshooting
{
    /// <summary>
    /// Painting panel for editor view.
    /// </summary>
    public partial class ScreenShotPanel : Form
    {
        /// <summary>
        /// Button for enabling painting mode.
        /// </summary>
        public ImageButton EnablePaintingModeButton { get; }

        public ImageButton SaveScreenShotButton { get; }

        /// <summary>
        /// Initializes painting panel view.
        /// </summary>
        public ScreenShotPanel()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(166, 56);

            var buttonSize = new Size(80, 50);
            var imageSize = new Size(40, 40);

            var paintImage = new Bitmap(Resources.Properties.Resources.Paint);
            var saveImage = new Bitmap(Resources.Properties.Resources.Upload);

            EnablePaintingModeButton = new ImageButton(buttonSize, paintImage, scaleImageSize: imageSize);

            SaveScreenShotButton = new ImageButton(buttonSize, saveImage, scaleImageSize: imageSize);

            Content = new StackLayout
            {
                Padding = 3,
                Orientation = Orientation.Horizontal,
                Items =
                {
                    EnablePaintingModeButton,
                    SaveScreenShotButton
                }
            };

        }
    }
}
