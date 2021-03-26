using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Screenshooting.Common;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Screenshooting.Support
{
    /// <summary>
    /// Image file helper.
    /// </summary>
    internal static class ImageFileHelper
    {
        /// <summary>
        /// Get file from bitmap.
        /// </summary>
        internal static async Task<File> GetFileFromBitmapAsync(Bitmap image, CancellationToken cancellationToken = default)
        {
            var stringDate = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss", CultureInfo.InvariantCulture);
            var baseName = string.Format("RedShot-Image-{0}", stringDate);
            var path = System.IO.Path.Combine(ScreenshootingProperties.ImagesFolder, $"{baseName}.png");

            await image.SaveAsync(path, ImageFormat.Png, cancellationToken);
            return new File(path, FileType.Image);
        }
    }
}
