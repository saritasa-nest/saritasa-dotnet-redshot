using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.RecordingRedShot.Views
{
    /// <summary>
    /// Painting panel for editor view.
    /// </summary>
    public partial class RecordManagePanel : Form
    {
        public ImageButton StartRecordingButton { get; }

        /// <summary>
        /// Initializes painting panel view.
        /// </summary>
        public RecordManagePanel()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(84, 54);

            StartRecordingButton = new ImageButton(new Size(80, 50), new Bitmap(Resources.Properties.Resources.Record), scaleImageSize: new Size(40, 40))
            {
                ToolTip = "Open recording view"
            };

            Content = new StackLayout
            {
                Padding = 2,
                Orientation = Orientation.Horizontal,
                Items =
                {
                    StartRecordingButton
                }
            };
        }
    }
}
