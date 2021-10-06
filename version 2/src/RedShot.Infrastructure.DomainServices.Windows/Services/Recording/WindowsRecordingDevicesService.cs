using RedShot.Infrastructure.Domain.Recording.Devices;
using RedShot.Infrastructure.DomainServices.Common.CliManagement;
using RedShot.Infrastructure.DomainServices.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Recording
{
    public class WindowsRecordingDevicesService : IRecordingDevicesService
    {
        private readonly IRecordingAppInstaller recordingApplication;
        private readonly CliApplication cliApplication;

        public WindowsRecordingDevicesService(IRecordingAppInstaller recordingApplication)
        {
            this.recordingApplication = recordingApplication;

            cliApplication = new CliApplication();
        }

        public async Task<RecordingDevices> GetRecordingDevicesAsync()
        {
            var fileName = await recordingApplication.GetRecorderFileNameAsync();

            await cliApplication.StartAsync(fileName, "-list_devices true -f dshow -i dummy");
            await cliApplication.WaitForExitAsync();

            var output = cliApplication.Output;
            bool isVideo = true;
            var regex = new Regex(@"\[dshow @ \w+\]  ""(.+)""", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            var lines = ArgumentsHelper.SplitLines(output);
            var recordingDevices = new RecordingDevices();

            foreach (var line in lines)
            {
                if (line.Contains("] DirectShow video devices", StringComparison.InvariantCulture))
                {
                    isVideo = true;
                    continue;
                }

                if (line.Contains("] DirectShow audio devices", StringComparison.InvariantCulture))
                {
                    isVideo = false;
                    continue;
                }

                var match = regex.Match(line);

                if (match.Success)
                {
                    string value = match.Groups[1].Value;

                    if (isVideo)
                    {
                        recordingDevices.VideoDevices.Add(new Device(value, value));
                    }
                    else
                    {
                        recordingDevices.AudioDevices.Add(new Device(value, value));
                    }
                }
            }

            return recordingDevices;
        }
    }
}
