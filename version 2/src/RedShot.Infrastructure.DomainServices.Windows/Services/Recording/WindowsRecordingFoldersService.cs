using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Recording
{
    public class WindowsRecordingFoldersService : IRecordingFoldersService
    {
        public string GetVideosFolder()
        {
            var myVideos = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            var applicationVideosFolder = Path.Combine(myVideos, "RedShot");
            return Directory.CreateDirectory(applicationVideosFolder).FullName;
        }

        public string GetRecordingAppBinariesFolder()
        {
            var localData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return Directory.CreateDirectory(
                Path.Combine(localData, "FFmpeg"))
                .FullName;
        }
    }
}
