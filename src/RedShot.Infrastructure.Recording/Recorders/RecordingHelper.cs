﻿using System;
using System.IO;

namespace RedShot.Infrastructure.Recording.Recorders
{
    /// <summary>
    /// Recording helper.
    /// </summary>
    internal static class RecordingHelper
    {
        /// <summary>
        /// Get default video folder.
        /// </summary>
        public static string GetDefaultVideoFolder()
        {
            var systemVideoFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            var folderPath = Path.Combine(systemVideoFolder, "RedShot");
            return Directory.CreateDirectory(folderPath).FullName;
        }
    }
}
