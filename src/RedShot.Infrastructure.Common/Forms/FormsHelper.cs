using Eto.Drawing;
using Eto.Forms;
using SkiaSharp;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Forms helper.
    /// </summary>
    public static class FormsHelper
    {
        /// <summary>
        /// Returns control which contains void box.
        /// </summary>
        public static Control VoidBox(int size)
        {
            return new Panel
            {
                Size = new Size(size, size),
                Padding = size
            };
        }

        /// <summary>
        /// Returns control which contains void rectangle.
        /// </summary>
        public static Control VoidRectangle(int width, int height)
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Size = new Size(width, height),
            };
        }

        /// <summary>
        /// Return pointer cursor (circle pointer).
        /// </summary>
        public static Cursor GetPointerCursor(SKColor color, int radius)
        {
            var skImage = SkiaSharpHelper.GetPointerForPainting(color, radius);

            var bitmap = EtoDrawingHelper.GetEtoBitmapFromSkiaImage(skImage);

            return new Cursor(bitmap, new PointF(radius, radius));
        }

        /// <summary>
        /// Return cursor via specified icon and size.
        /// </summary>
        public static Cursor GetCursor(Bitmap icon, Size size, Point hotSpot = default)
        {
            var scaled = new Bitmap(icon, size.Width, size.Height, ImageInterpolation.High);

            if (hotSpot == default)
            {
                hotSpot = new Point(size.Width / 2, size.Height / 2);
            }

            return new Cursor(scaled, hotSpot);
        }

        /// <summary>
        /// Gives base stack (Label + control) in horizontal orientation.
        /// </summary>
        public static StackLayout GetBaseStack(string name, Control control, int nameWidth = 200, int controlWidth = 300)
        {
            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 5,
                Items =
                {
                    new StackLayout()
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Width = nameWidth,
                        Items =
                        {
                            new Label()
                            {
                                Text = name,
                            }
                        }
                    },
                    new StackLayout()
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Width = controlWidth,
                        Items =
                        {
                            control
                        }
                    },
                }
            };
        }
    }
}
