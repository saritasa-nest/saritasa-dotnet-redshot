using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Screenshooting.Common;

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
        internal static async Task<IFile> GetFileFromBitmapAsync(Bitmap image, CancellationToken cancellationToken = default)
        {
            var imageName = FormatManager.GetFormattedName();
            var stringDate = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss", CultureInfo.InvariantCulture);
            var baseName = string.Format("RedShot-Image-{0}", stringDate);
            var path = Path.Combine(ScreenshootingConstants.ImagesFolder, $"{baseName}.png");

            await image.SaveAsync(path, ImageFormat.Png, cancellationToken);
            return new ImageFile(image, path, imageName);
        }
    }
}
