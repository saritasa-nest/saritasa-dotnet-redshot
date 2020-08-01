using Eto.Forms;
using Eto.Drawing;

namespace RedShot.App
{
    public partial class PointPaintingView : Form
    {
        public Button SelectionModeEnabledButton { get; }

        public Button ClearButton { get; }

        public Button PaintingModeEnabledButton { get; }

        public PointPaintingView()
        {
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(180, 60);

            SelectionModeEnabledButton = new Button()
            {
                Text = "Select",
                Size = new Size(60, 60)
            };

            ClearButton = new Button()
            {
                Text = "Clear",
                Size = new Size(59, 59)
            };

            PaintingModeEnabledButton = new Button()
            {
                Text = "Paint",
                Size = new Size(59, 59)
            };

            Content = new StackLayout
            {
                Size = new Size(60, 180),
                Orientation = Orientation.Horizontal,
                Items =
                {
                    SelectionModeEnabledButton,
                    ClearButton,
                    PaintingModeEnabledButton
                }
            };

        }
    }
}
