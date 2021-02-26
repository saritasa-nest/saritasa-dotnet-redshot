using System;
using System.Runtime.InteropServices;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Recording.Common.Devices;
using RedShot.Infrastructure.Recording.Unix.MacOs;
using RedShot.Infrastructure.Recording.Unix.Linux;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// Unix recording service.
    /// </summary>
    public class RecordingService : IRecordingService
    {
        private readonly IRecordingService recordingService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordingService()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                recordingService = new MacOsRecordingService();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                recordingService = new LinuxRecordingService();
            }
            else
            {
                throw new PlatformNotSupportedException("System platform is not supported for recording.");
            }
        }

        /// <summary>
        /// Install FFmpeg binaries.
        /// </summary>
        public void InstallFFmpeg()
        {
            recordingService.InstallFFmpeg();
        }

        /// <summary>
        /// Check on existing FFmpeg binaries.
        /// </summary>
        public bool CheckFFmpeg()
        {
            return recordingService.CheckFFmpeg();
        }

        /// <inheritdoc />
        public IRecorder GetRecorder()
        {
            return recordingService.GetRecorder();
        }

        /// <inheritdoc />
        public RecordingDevices GetRecordingDevices()
        {
            return recordingService.GetRecordingDevices();
        }
    }
}
