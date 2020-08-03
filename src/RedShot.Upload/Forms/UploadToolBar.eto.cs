using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

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
        public UploadToolBarButton ClipBoardButton { get; private set; }

        /// <summary>
        /// Save to file button.
        /// </summary>
        public UploadToolBarButton SaveToFileButton { get; private set; }

        /// <summary>
        /// Save to ftp button.
        /// </summary>
        public UploadToolBarButton SaveToFtpButton { get; private set; }

        /// <summary>
        /// Close button.
        /// </summary>
        public UploadToolBarButton CloseButton { get; private set; }

        void InitializeComponent()
        {
            ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

            ClipBoardButton = new UploadToolBarButton("ClipB");
            SaveToFileButton = new UploadToolBarButton("File");
            SaveToFtpButton = new UploadToolBarButton("FTP");
            CloseButton = new UploadToolBarButton("Close");

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
