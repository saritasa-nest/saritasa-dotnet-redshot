using System;
using System.Text.RegularExpressions;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Abstractions;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

namespace RedShot.Infrastructure.Recording.Recorders.MacOs
{
    /// <summary>
    /// Mac OS recording service.
    /// </summary>
    internal class MacOsRecordingService : BaseRecordingService
    {
        private static readonly Regex deviceExpression = new Regex(@"\[AVFoundation input device @ \w+\] \[([0-9]+)] (.+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <inheritdoc />
        protected override string FfmpegBinaryName => "ffmpeg";

        /// <inheritdoc />
        protected override string BinariesUrl => "https://evermeet.cx/ffmpeg/get/zip";

        /// <inheritdoc />
        public override IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();
            var configuration = ConfigurationManager.GetSection<FFmpegConfiguration>();
            return new MacOsRecorder(configuration, GetFfmpegPath(), RecordingHelper.GetDefaultVideoFolder());
        }

        /// <inheritdoc />
        public override RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFfmpegPath());

            var recordingDevices = new RecordingDevices();

            cli.Run("-f avfoundation -list_devices true -i \"\"");
            cli.WaitForExit();

            string output = cli.Output.ToString();
            bool isVideo = true;

            var lines = ArgumentsHelper.SplitLines(output);
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
                        recordingDevices.VideoDevices.Add(new Device(deviceName, deviceIndex));
                    }
                    else
                    {
                        recordingDevices.AudioDevices.Add(new Device(deviceName, deviceIndex));
                    }
                }
            }

            return new RecordingDevices();
        }
    }
}