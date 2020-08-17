using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers.Forms;

namespace RedShot.Recording.Views
{
    /// <summary>
    /// Painting panel for editor view.
    /// </summary>
    public partial class RecordManagePanel : Form
    {
        public DefaultButton StartRecordingButton { get; }

        /// <summary>
        /// Initializes painting panel view.
        /// </summary>
        public RecordManagePanel()
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Size = new Size(80, 50);

            StartRecordingButton = new DefaultButton("Start", 80, 50);

            Content = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Items =
                {
                    StartRecordingButton
                }
            };

        }
    }
}
