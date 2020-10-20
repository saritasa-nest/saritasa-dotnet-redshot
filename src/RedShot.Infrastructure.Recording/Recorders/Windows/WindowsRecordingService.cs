using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Recorders;

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
        protected override string BinariesUrl => "https://github.com/BtbN/FFmpeg-Builds/releases/download/autobuild-2020-10-11-12-31/ffmpeg-N-99531-g2be3eb7f77-win64-gpl.zip";

        /// <inheritdoc />
        public override IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();

            var options = ConfigurationManager.GetSection<FFmpegConfiguration>().Options;

            return new WindowsRecorder(options, GetFullFfmpegPath());
        }

        /// <inheritdoc />
        public override IRecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFullFfmpegPath());

            var videoDevices = new List<Device>();
            var audioDevices = new List<Device>();

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
                        videoDevices.Add(new Device(value, value));
                    }
                    else
                    {
                        audioDevices.Add(new Device(value, value));
                    }
                }
            }

            return new RecordingDevices(videoDevices, audioDevices);
        }
    }
}
