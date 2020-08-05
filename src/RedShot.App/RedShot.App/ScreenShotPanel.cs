using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers.Forms;

namespace RedShot.App
{
    /// <summary>
    /// Painting panel for editor view.
    /// </summary>
    public partial class ScreenShotPanel : Form
    {
        /// <summary>
        /// Button for enabling painting mode.
        /// </summary>
        public DefaultButton EnablePaintingModeButton { get; }

        public DefaultButton SaveScreenShotButton { get; }

        /// <summary>
        /// Initializes painting panel view.
        /// </summary>
        public ScreenShotPanel()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(160, 50);

            EnablePaintingModeButton = new DefaultButton("Paint", 80, 50);

            SaveScreenShotButton = new DefaultButton("Save", 80, 50);

            Content = new StackLayout
            {
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
