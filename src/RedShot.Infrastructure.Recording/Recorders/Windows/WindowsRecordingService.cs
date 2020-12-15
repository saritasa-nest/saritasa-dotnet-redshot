using System;
using System.Text.RegularExpressions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Recorders;
using RedShot.Infrastructure.Recording.Abstractions;

namespace RedShot.Recording.Recorders.Windows
{
    /// <summary>
    /// Windows recorder service.
    /// </summary>
    internal class WindowsRecordingService : BaseRecordingService
    {
        /// <inheritdoc />
        protected override string FfmpegBinaryName => "ffmpeg.exe";

        /// <inheritdoc />
        protected override string BinariesUrl => ConfigurationManager.AppSettings.FfmpegWindowsDownloadPath;

        /// <inheritdoc />
        public override IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();

            var options = ConfigurationManager.GetSection<FFmpegConfiguration>();

            return new WindowsRecorder(options, GetFullFfmpegPath());
        }

        /// <inheritdoc />
        public override RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFullFfmpegPath());

            var recordingDevices = new RecordingDevices();

            cli.Run("-list_devices true -f dshow -i dummy");
            cli.WaitForExit();

            string output = cli.Output.ToString();
            string[] lines = GetLines(output);
            bool isVideo = true;
            Regex regex = new Regex(@"\[dshow @ \w+\]  ""(.+)""", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            foreach (string line in lines)
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

                Match match = regex.Match(line);

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
