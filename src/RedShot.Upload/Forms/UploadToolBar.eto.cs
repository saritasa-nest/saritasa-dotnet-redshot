using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;
using RedShot.Helpers.Forms;
using System.IO;
using System.Reflection;
using RedShot.Upload.Properties;

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
        public ImageButton ClipBoardButton { get; private set; }

        /// <summary>
        /// Save to file button.
        /// </summary>
        public ImageButton SaveToFileButton { get; private set; }

        /// <summary>
        /// Save to ftp button.
        /// </summary>
        public ImageButton SaveToFtpButton { get; private set; }

        /// <summary>
        /// Close button.
        /// </summary>
        public ImageButton CloseButton { get; private set; }

        void InitializeComponent()
        {
            ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

            var boardSize = ScreenHelper.GetSixteenthPartOfDisplay();

            var buttonSize = new Size(boardSize, boardSize);

            var clipBImage = new Bitmap(Resources.form);
            var closeImage = new Bitmap(Resources.close);
            var ftpImage = new Bitmap(Resources.ftp);
            var fileImage = new Bitmap(Resources.download);

            ClipBoardButton = new ImageButton(buttonSize, clipBImage);
            SaveToFileButton = new ImageButton(buttonSize, fileImage);
            SaveToFtpButton = new ImageButton(buttonSize, ftpImage);
            CloseButton = new ImageButton(buttonSize, closeImage);

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
