using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Recording
{
    public interface IRecordingAppInstaller
    {
        Task InstallAsync();

        Task<string> GetRecorderFileNameAsync();

        Task<bool> CheckRecorderExistingAsync();
    }
}
