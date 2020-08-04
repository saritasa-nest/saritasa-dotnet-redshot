using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;
using RedShot.Helpers.Forms;

namespace RedShot.Upload.Forms
{
    /// <summary>
    /// Toolbar for uploadbar.
    /// </summary>
    internal partial class UploadToolBar : Panel
    {
        /// <summary>
        /// Clipbaord upload button.
        /// </summary>
        public DefaultButton ClipBoardButton { get; private set; }

        /// <summary>
        /// Save to file button.
        /// </summary>
        public DefaultButton SaveToFileButton { get; private set; }

        /// <summary>
        /// Save to ftp button.
        /// </summary>
        public DefaultButton SaveToFtpButton { get; private set; }

        /// <summary>
        /// Close button.
        /// </summary>
        public DefaultButton CloseButton { get; private set; }

        void InitializeComponent()
        {
            ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

            var buttonSize = ScreenHelper.GetSixteenthPartOfDisplay();

            ClipBoardButton = new DefaultButton("ClipB", buttonSize, buttonSize);
            SaveToFileButton = new DefaultButton("File", buttonSize, buttonSize);
            SaveToFtpButton = new DefaultButton("FTP", buttonSize, buttonSize);
            CloseButton = new DefaultButton("Close", buttonSize, buttonSize);

            Content = new StackLayout
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    SaveToFtpButton,
                    ClipBoardButton,
                    SaveToFileButton,
                    CloseButton,
                }
            };
        }
    }
}
