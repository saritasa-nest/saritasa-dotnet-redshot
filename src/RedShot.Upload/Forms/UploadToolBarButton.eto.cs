using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

namespace RedShot.Upload.Forms
{
	partial class UploadToolBarButton : Panel
	{
		public event EventHandler<EventArgs> Clicked;
		void InitializeComponent()
		{
			Width = ScreenHelper.GetSixteenthPartOfDisplay();
			Height = ScreenHelper.GetSixteenthPartOfDisplay();

			var btn = new Button();
			btn.Width = Width;
			btn.Height = Height;
			btn.Text = name;
            btn.Click += Btn_Click;

			Content = new StackLayout
			{
				Items =
				{
					btn
				}
			};

		}

        private void Btn_Click(object sender, EventArgs e)
        {
			Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
