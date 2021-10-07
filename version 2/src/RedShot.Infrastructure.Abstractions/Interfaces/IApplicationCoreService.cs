using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IApplicationCoreService
    {
        Task TakeScreenshotAsync();

        Task RecordVideoAsync();

        Task OpenConfigurationAsync();

        Task OpenLastFileAsync();

        Task SendFeedBackAsync();

        void CloseApplication();
    }
}
