using Eto.Drawing;
using Eto.Forms;
using SkiaSharp;

namespace RedShot.Infrastructure.Common.Forms
{
    public static class FormsHelper
    {
        public static Control VoidBox(int size)
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Size = new Eto.Drawing.Size(size, size),
                Padding = size
            };
        }

        public static Control VoidRectangle(int width, int height)
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Size = new Size(width, height),
            };
        }

        public static Cursor GetPointerCursor(Color color, int radius)
        {
            var skColor = SkiaSharpHelper.GetSKColorFromEtoColor(color);

            var skImage = SkiaSharpHelper.GetPointerForPainting(skColor, radius);

            var bitmap = EtoDrawingHelper.GetEtoBitmapFromSkiaImage(skImage);

            return new Cursor(bitmap, new PointF(radius, radius));
        }

        public static Cursor GetPointerCursor(SKColor color, int radius)
        {
            var skImage = SkiaSharpHelper.GetPointerForPainting(color, radius);

            var bitmap = EtoDrawingHelper.GetEtoBitmapFromSkiaImage(skImage);

            return new Cursor(bitmap, new PointF(radius, radius));
        }

        public static Point GetCenterLocation(Size size)
        {
            var center = ScreenHelper.GetCentralCoordsOfScreen();

            var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);

            if (location.X >= 0 && location.Y >= 0)
            {
                return location;
            }
            else
            {
                return center;
            }
        }

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
