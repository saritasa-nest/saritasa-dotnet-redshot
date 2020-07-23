using Eto.Drawing;
using SkiaSharp;
using System.IO;

namespace RedShot.Helpers
{
    public static class SkiaSharpHelper
    {
        public static SKBitmap ConvertFromEtoBitmap(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bitmap);
                ms.Seek(0, SeekOrigin.Begin);

                return SKBitmap.Decode(ms);
            }
        }
    }
}
