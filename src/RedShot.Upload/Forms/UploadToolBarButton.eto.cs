using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

namespace RedShot.Upload.Forms
{
	partial class UploadToolBarButton : Panel
	{
		void InitializeComponent()
		{
			Width = ScreenHelper.GetSixteenthPartOfDisplay();
			Height = ScreenHelper.GetSixteenthPartOfDisplay();

			BackgroundColor = Colors.Aqua;

			Content = new StackLayout
			{
				Items =
				{
					name
				}
			};

		}
	}
}
