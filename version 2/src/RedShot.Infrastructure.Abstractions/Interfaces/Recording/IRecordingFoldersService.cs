using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Recording
{
    public interface IRecordingFoldersService
    {
        string GetVideosFolder();

        string GetRecordingAppBinariesFolder();
    }
}
