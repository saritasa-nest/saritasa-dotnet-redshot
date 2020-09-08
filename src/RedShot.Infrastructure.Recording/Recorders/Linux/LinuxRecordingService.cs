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
        public IRecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var devices = new RecordingDevices();

            return devices;
        }

        /// <inheritdoc />
        public void InstallFFmpeg()
        {
            if (CheckFFmpeg())
            {
                throw new Exception("FFmpeg is installed already!");
            }

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
