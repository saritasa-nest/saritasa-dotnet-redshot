using Eto.Forms;
using Eto.Drawing;
using System.IO;
using RedShot.ScreenshotCapture;
using System.Threading.Tasks;

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

			imageview.Image = sourceImage = SetDisplayImage(); 
			UpdateContent();

			Style = "mystyle";

			WindowState = WindowState.Maximized;
		}
		private void UpdateContent()
        {
			var layout = new DynamicLayout();
			layout.Add(imageview);
			Content = layout;
		}
		private void ClearImageView()
        {
			image = sourceImage.Clone();
			imageview.Image = image;
		}

		private void EditorView_MouseMove(object sender, MouseEventArgs e)
		{
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
					RenderRectangle();
					capturing = false;

					//Some save functions
					Close();
				}
                else
                {
					startLocation = e.Location;
					capturing = true;
				}
			}
			else if (e.Buttons == MouseButtons.Alternate)
            {
                if (capturing)
                {
					capturing = false;
				}
            }
			ClearImageView();
		}

		private void RenderRectangle()
		{
			image = sourceImage.Clone();
			using (var graphics = new Graphics(image))
			{
				using (var brush = new SolidBrush(Color.FromRgb(int.Parse("808080", System.Globalization.NumberStyles.HexNumber))))
				{
					if (startLocation.Y < endLocation.Y)
					{
						graphics.FillRectangle(brush, new RectangleF(startLocation, endLocation));
					}
					else
					{
						graphics.FillRectangle(brush, new RectangleF(endLocation, startLocation));
					}
					imageview.Image = image;
					UpdateContent();
				}
			}
			
		}
		private Bitmap SetDisplayImage()
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
