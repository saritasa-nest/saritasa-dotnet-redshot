using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Forms
{
    internal class UploadToolBar : Panel
    {
        public UploadToolBar()
        {
            InitializeComponent();
        }

        public ImageButton UploadersButton { get; private set; }

        /// <summary>
        /// Close button.
        /// </summary>
        public ImageButton CloseButton { get; private set; }

        void InitializeComponent()
        {
            ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

            var boardSize = ScreenHelper.GetSixteenthPartOfDisplay();

            var buttonSize = new Size(boardSize, boardSize);

            var uploadImage = new Bitmap(RedShot.Resources.Properties.Resources.Upload);

            var closeImage = new Bitmap(RedShot.Resources.Properties.Resources.Close);

            UploadersButton = new ImageButton(buttonSize, uploadImage);
            CloseButton = new ImageButton(buttonSize, closeImage);

            Content = new StackLayout
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Bottom,
                Items =
                {
                    UploadersButton,
                    CloseButton,
                }
            };
        }
    }
}
