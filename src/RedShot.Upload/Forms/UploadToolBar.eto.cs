using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

namespace RedShot.Upload.Forms
{
	partial class UploadToolBar : Panel
	{
		void InitializeComponent()
		{
			ClientSize = new Size(ScreenHelper.GetSixteenthPartOfDisplay(), ScreenHelper.GetSixteenthPartOfDisplay() * 4);

			Content = new StackLayout
			{
				Orientation = Orientation.Vertical,
				Items =
				{
					new UploadToolBarButton("Cloud"),
					new UploadToolBarButton("ClipB"),
					new UploadToolBarButton("File"),
					new UploadToolBarButton("Del"),
				}
			};
		}
	}
}
