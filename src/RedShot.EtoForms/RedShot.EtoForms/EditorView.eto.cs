using System;
using Eto.Forms;
using Eto.Drawing;
using System.IO;
using RedShot.ScreenshotCapture;

namespace RedShot.EtoForms
{
	partial class EditorView : Form
	{
		ImageView imageview = new ImageView();
		Bitmap sourceImage;
		Bitmap image;
		PointF startLocation;
		PointF endLocation;
		bool capturing;
		void InitializeComponent()
		{
			this.MouseDown += EditorView_MouseDown;
			this.MouseMove += EditorView_MouseMove;

			var rect = ScreenshotCapture.ScreenShot.GetMainWindowSize();
			var size = new Size(rect.Width, rect.Height);

			ClientSize = size;

			sourceImage = SetDisplayImage();

			ClearImageView();

			var layout = new DynamicLayout();

			layout.Add(imageview);
			Content = layout;

			Style = "mystyle";

			WindowState = WindowState.Maximized;
		}
		private void ClearImageView()
        {
			image = sourceImage.Clone();
			imageview.Image = image;

			var layout = new DynamicLayout();

			layout.Add(imageview);
			Content = layout;
		}

		private void EditorView_MouseMove(object sender, MouseEventArgs e)
		{
			ClearImageView();
			if (capturing)
			{
				endLocation = e.Location;
				RenderRectangle();
			}
		}

		private void EditorView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Buttons == MouseButtons.Primary)
			{
				if (capturing)
				{
					endLocation = e.Location;
					ClearImageView();
					RenderRectangle();
					capturing = false;
				}
                else
                {
					startLocation = e.Location;
					capturing = true;
				}
			}
		}

		private void RenderRectangle()
		{
			using (var graphics = new Graphics(image))
			{
				using (var brush = new SolidBrush(Color.FromRgb(int.Parse("808080", System.Globalization.NumberStyles.HexNumber))))
				{                    
					if(startLocation.Y < endLocation.Y)
                    {
						graphics.FillRectangle(brush, new RectangleF(startLocation, endLocation));
					}
                    else
                    {
						graphics.FillRectangle(brush, new RectangleF(endLocation, startLocation));
					}
					imageview.Image = image;
				}
			}
		}

		//using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))

		//graphics.FillRectangle(myBrush, new Rectangle(0, 0, 200, 300)); // whatever
		// and so on...
		// myBrush will be disposed at this line
		//bitmap.Save(fileName);
	 // graphics will be disposed at this line

		public Bitmap SetDisplayImage()
		{
			using (var ms = new MemoryStream())
			{
				var bitmap = ScreenShot.TakeScreenshot();
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
				ms.Seek(0, SeekOrigin.Begin);

				return new Eto.Drawing.Bitmap(ms);
			}
		}
	}	
}
