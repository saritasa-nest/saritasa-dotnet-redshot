using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

namespace RedShot.Upload.Forms
{
	partial class UploadBar : Form
	{
		bool isClosing;
		void InitializeComponent()
		{
			WindowStyle = WindowStyle.None;
			ShowInTaskbar = false;

			var size = ScreenHelper.GetMiniSizeDisplay();
			var sixteenpart = ScreenHelper.GetSixteenthPartOfDisplay();

			this.Location = ScreenHelper.GetStartPointForUploadView();
			var timer = new UITimer();
			timer.Interval = 4;
            timer.Elapsed += Timer_Elapsed;
			timer.Start();

			Size = new Size(sixteenpart + size.Width, size.Height);
			var imageview = new ImageView();
			imageview.Image = imageOnUpload;
			imageview.Size = size;
			Topmost = true;

			Content = new StackLayout
			{
				Orientation = Orientation.Horizontal,
				Items =
				{
					new UploadToolBar(),
					imageview
				}
			};			
		}

        private void Timer_Elapsed(object sender, EventArgs e)
        {
			if(isClosing == false)
            {
				isClosing = true;
				var timer = new UITimer();
				timer.Interval = 0.1;
                timer.Elapsed += Timer_Elapsed1;
				timer.Start();
            }
            else
            {
				Close();
            }
			
        }

        private void Timer_Elapsed1(object sender, EventArgs e)
        {
			Opacity = Opacity / 1.5;
        }
    }
}
