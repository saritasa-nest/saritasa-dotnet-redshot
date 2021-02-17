using System;
using Eto.Forms;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Common.Recorders;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Recording.Common.Devices;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Infrastructure.Recording.Unix.Linux
{
    /// <summary>
    /// Linux recorder service.
    /// </summary>
    internal class LinuxRecordingService : IRecordingService
    {
        private readonly string ffmpegName;
        private readonly CliManager simpleCliManager;

        /// <summary>
        /// Initializes Linux recording service.
        /// </summary>
        public LinuxRecordingService()
        {
            ffmpegName = "ffmpeg";
            simpleCliManager = new CliManager(ffmpegName);
        }

        /// <inheritdoc />
        public IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();
            var configuration = ConfigurationManager.GetSection<FFmpegConfiguration>();
            return new LinuxRecorder(configuration, RecordingHelper.GetDefaultVideoFolder());
        }

        /// <inheritdoc />
        public bool CheckFFmpeg()
        {
            try
            {
                simpleCliManager.Run(" -h");
                return simpleCliManager.Output.ToString().Contains("ffmpeg version");
            }
            // Can throw an exception when can not find FFmpeg in the OS.
            catch
            {
                return false;
            }
        }

        /// <inheritdoc />
        public RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var devices = new RecordingDevices();

            return devices;
        }

        /// <inheritdoc />
        public void InstallFFmpeg()
        {
            MessageBox.Show("Download FFmpeg package to your system before recording video.", MessageBoxButtons.OK, MessageBoxType.Information);
        }

        private void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new DllNotFoundException($"FFmpeg binary is not found!");
            }
        }
    }
}
