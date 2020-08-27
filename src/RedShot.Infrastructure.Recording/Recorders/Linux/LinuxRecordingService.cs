using System;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Devices;
using RedShot.Infrastructure.Recording;

namespace RedShot.Recording.Recorders.Linux
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

            var options = ConfigurationManager.GetSection<FFmpegConfiguration>().Options;

            return new LinuxRecorder(options);
        }

        /// <inheritdoc />
        public bool CheckFFmpeg()
        {
            simpleCliManager.Run(" -h");
            return simpleCliManager.Output.ToString().Contains("ffmpeg version");
        }

        /// <inheritdoc />
        public IRecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var devices = new RecordingDevices();

            return devices;
        }

        /// <inheritdoc />
        public bool InstallFFmpeg()
        {
            if (CheckFFmpeg())
            {
                throw new Exception("FFmpeg is installed already!");
            }

            MessageBox.Show("Download ffmpeg package to your system before recording video", MessageBoxButtons.OK, MessageBoxType.Information);
            return false;
        }

        private void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new DllNotFoundException($"ffmpeg binary is not found!");
            }
        }
    }
}
