using System;
using Eto.Forms;
using RedShot.Helpers;
using RedShot.Helpers.Ffmpeg.Devices;
using RedShot.Helpers.Ffmpeg.Options;

namespace RedShot.Recording.Recorders.Linux
{
    public class LinuxRecordingManager : IRecordingManager
    {
        private readonly string ffmpegName;

        private readonly CliManager simpleCliManager;

        public LinuxRecordingManager()
        {
            ffmpegName = "ffmpeg";
            simpleCliManager = new CliManager(ffmpegName);
        }

        public IRecorder GetRecorder(FFmpegOptions options)
        {
            ThrowIfNotFoundFfmpegBinary();

            return new LinuxRecorder(options);
        }

        public bool CheckFFmpeg()
        {
            simpleCliManager.Run(" -h");
            return simpleCliManager.Output.ToString().Contains("ffmpeg version");
        }

        public RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var devices = new RecordingDevices();

            return devices;
        }

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
