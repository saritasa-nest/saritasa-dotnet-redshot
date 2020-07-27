using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers;

namespace RedShot.Upload.Forms
{
	partial class UploadBar : Form
	{
		bool isFocused;
		bool canClose = true;
		bool blocked;
		UITimer checkTimer;
		UITimer closingTimer;
		UploadToolBar toolBar;

		void InitializeComponent()
		{
			BackgroundColor = Colors.FloralWhite;

			WindowStyle = WindowStyle.None;
			ShowInTaskbar = false;

			var size = ScreenHelper.GetMiniSizeDisplay();
			var sixteenpart = ScreenHelper.GetSixteenthPartOfDisplay();

			this.Location = ScreenHelper.GetStartPointForUploadView();

			checkTimer = new UITimer();
			checkTimer.Interval = 5;
			checkTimer.Elapsed += Timer_Elapsed;
			checkTimer.Start();

			closingTimer = new UITimer();
			closingTimer.Interval = 0.1;
			closingTimer.Elapsed += Timer_Elapsed1;

			Size = new Size(sixteenpart + size.Width, size.Height);
			var imageview = new ImageView();
			imageview.Image = imageOnUpload;
			imageview.Size = size;
			Topmost = true;

			toolBar = new UploadToolBar();

			Content = new StackLayout
			{
				Orientation = Orientation.Horizontal,
				Items =
				{
					toolBar,
					imageview
				}
			};

            this.MouseMove += UploadBar_MouseMove;
            toolBar.ClipBoardButton.Clicked += ClipBoardButton_Clicked;
            toolBar.SaveToFileButton.Clicked += SaveToFileButton_Clicked;
		}

        private void SaveToFileButton_Clicked(object sender, EventArgs e)
        {
			blocked = true;
			var res = UploadManager.UploadToFile(imageOnUpload, this);
			blocked = false;
		}

        private void ClipBoardButton_Clicked(object sender, EventArgs e)
        {
			UploadManager.UploadToClipboard(imageOnUpload);	
		}

        private void UploadBar_MouseMove(object sender, MouseEventArgs e)
        {
			RefreshTimer();
		}

		private void RefreshTimer()
        {
			isFocused = true;
			canClose = false;
			closingTimer.Stop();
			Opacity = 1;
		}

        private void Timer_Elapsed(object sender, EventArgs e)
        {
			if (blocked)
            {
				RefreshTimer();
				return;
            }
			else if (isFocused)
            {
				isFocused = false;
				return;
            }

			if(canClose == false)
            {
				closingTimer.Start();
				canClose = true;
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
