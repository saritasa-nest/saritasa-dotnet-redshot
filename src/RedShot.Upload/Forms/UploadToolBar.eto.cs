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
		void InitializeComponent()
		{
			ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

			ClipBoardButton = new UploadToolBarButton("ClipB");
			SaveToFileButton = new UploadToolBarButton("File");

			Content = new StackLayout
			{
				Orientation = Orientation.Vertical,
				Items =
				{
					new UploadToolBarButton("Cloud"),
					ClipBoardButton,
					SaveToFileButton,
					new UploadToolBarButton("Del"),
				}
			};
		}
    }
}
