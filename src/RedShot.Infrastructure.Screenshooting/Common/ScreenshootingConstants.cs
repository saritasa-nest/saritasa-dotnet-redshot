using System;
using System.IO;

namespace RedShot.Infrastructure.Screenshooting.Common
{
    /// <summary>
    /// Screen shooting constants.
    /// </summary>
    internal static class ScreenshootingConstants
    {
        /// <summary>
        /// Images folder.
        /// </summary>
        internal static string ImagesFolder { get; }

        static ScreenshootingConstants()
        {
            ImagesFolder = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;
        }
    }
}
