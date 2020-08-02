using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

namespace RedShot.App
{
    public partial class PointPaintingView : Form
    {
        public Button SelectionModeEnabledButton { get; }

        public Button ClearButton { get; }

        public Button PaintingModeEnabledButton { get; }

        public PointPaintingView()
        {
            ShowInTaskbar = false;
            BackgroundColor = StylesHelper.BackgroundColor;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(180, 60);

            var buttonSize = new Size(60, 60);

            SelectionModeEnabledButton = new Button()
            {
                Text = "Select",
                TextColor = StylesHelper.TextColor,
                Size = buttonSize
            };

            ClearButton = new Button()
            {
                Text = "Clear",
                TextColor = StylesHelper.TextColor,
                Size = buttonSize
            };

            PaintingModeEnabledButton = new Button()
            {
                Text = "Paint",
                TextColor = StylesHelper.TextColor,
                Size = buttonSize
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
