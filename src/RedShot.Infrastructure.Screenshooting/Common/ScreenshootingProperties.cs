using System;
using System.IO;

namespace RedShot.Infrastructure.Screenshooting.Common
{
    /// <summary>
    /// Screen shooting properties.
    /// </summary>
    internal static class ScreenshootingProperties
    {
        /// <summary>
        /// Images folder.
        /// </summary>
        internal static string ImagesFolder { get; }

        static ScreenshootingProperties()
        {
            ImagesFolder = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;
        }
    }
}
