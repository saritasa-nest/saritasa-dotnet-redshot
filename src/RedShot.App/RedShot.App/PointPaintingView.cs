using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers.Forms;

namespace RedShot.App
{
    /// <summary>
    /// Painting panel for editor view.
    /// </summary>
    public partial class PointPaintingView : Form
    {
        /// <summary>
        /// Button for enabling selection mode.
        /// </summary>
        public DefaultButton SelectionModeEnabledButton { get; }

        /// <summary>
        /// Button for clearing paintings.
        /// </summary>
        public DefaultButton ClearButton { get; }

        /// <summary>
        /// Button for enabling painting mode.
        /// </summary>
        public DefaultButton PaintingModeEnabledButton { get; }

        public DefaultButton SaveScreenShotButton { get; }

        /// <summary>
        /// Initializes painting panel view.
        /// </summary>
        public PointPaintingView()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(240, 60);

            var buttonSize = 60;

            SelectionModeEnabledButton = new DefaultButton("Select", buttonSize, buttonSize);

            ClearButton = new DefaultButton("Clear", buttonSize, buttonSize);

            PaintingModeEnabledButton = new DefaultButton("Paint", buttonSize, buttonSize);

            SaveScreenShotButton = new DefaultButton("Save", buttonSize, buttonSize);

            Content = new StackLayout
            {
                Size = new Size(240, 60),
                Orientation = Orientation.Horizontal,
                Items =
                {
                    SelectionModeEnabledButton,
                    ClearButton,
                    PaintingModeEnabledButton,
                    SaveScreenShotButton
                }
            };

        }
    }
}
