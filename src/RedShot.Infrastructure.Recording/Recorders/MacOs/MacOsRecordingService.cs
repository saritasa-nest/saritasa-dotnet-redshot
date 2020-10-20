using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

namespace RedShot.Infrastructure.Recording.Recorders.MacOs
{
    internal class MacOsRecordingService : BaseRecordingService
    {
        private static readonly Regex deviceExpression = new Regex(@"\[AVFoundation input device @ \w+\] \[([0-9]+)] (.+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        protected override string FfmpegBinaryName => "ffmpeg";

        protected override string BinariesUrl => "https://evermeet.cx/ffmpeg/get/zip";

        /// <inheritdoc />
        public override IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();

            var options = ConfigurationManager.GetSection<FFmpegConfiguration>().Options;

            return new MacOsRecorder(options, GetFullFfmpegPath());
        }

        /// <inheritdoc />
        public override IRecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFullFfmpegPath());

            var videoDevices = new List<Device>();
            var audioDevices = new List<Device>();

            cli.Run("-f avfoundation -list_devices true -i \"\"");
            cli.WaitForExit();

            string output = cli.Output.ToString();
            string[] lines = GetLines(output);
            bool isVideo = true;

            foreach (string line in lines)
            {
                if (line.Contains("] AVFoundation video devices", StringComparison.InvariantCulture))
                {
                    isVideo = true;
                    continue;
                }

                if (line.Contains("] AVFoundation audio devices", StringComparison.InvariantCulture))
                {
                    isVideo = false;
                    continue;
                }

                var match = deviceExpression.Match(line);

                if (match.Success)
                {
                    var deviceIndex = match.Groups[1].Value;
                    var deviceName = match.Groups[2].Value;

                    if (isVideo)
                    {
                        videoDevices.Add(new Device(deviceName, deviceIndex));
                    }
                    else
                    {
                        audioDevices.Add(new Device(deviceName, deviceIndex));
                    }
                }
            }

            return new RecordingDevices(videoDevices, audioDevices);
        }
    }
}