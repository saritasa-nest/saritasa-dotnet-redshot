using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;
using System.Linq;

namespace RedShot.Upload.Forms
{
	partial class UploadToolBar : Panel
	{
		public UploadToolBarButton ClipBoardButton;
		public UploadToolBarButton SaveToFileButton;
		public UploadToolBarButton SaveToFtpButton;
		void InitializeComponent()
		{
			ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

			ClipBoardButton = new UploadToolBarButton("ClipB");
			SaveToFileButton = new UploadToolBarButton("File");
			SaveToFtpButton = new UploadToolBarButton("FTP");

			Content = new StackLayout
			{
				Orientation = Orientation.Vertical,
				Items =
				{
					SaveToFtpButton,
					ClipBoardButton,
					SaveToFileButton,
					new UploadToolBarButton("Del"),
				}
			};
		}
    }
}