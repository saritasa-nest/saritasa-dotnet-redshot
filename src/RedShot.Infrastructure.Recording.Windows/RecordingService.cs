using System;
using System.Text.RegularExpressions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration.Models;
using RedShot.Infrastructure.Recording.Common.Recorders;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using RedShot.Infrastructure.Recording.Common.Devices;
using RedShot.Infrastructure.Configuration.Models.Recording;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// Windows recording service.
    /// </summary>
    public class RecordingService : BaseRecordingService
    {
        /// <inheritdoc />
        protected override string FfmpegBinaryName => "ffmpeg.exe";

        /// <inheritdoc />
        protected override string BinariesUrl => AppSettings.Instance.FfmpegWindowsDownloadPath;

        /// <inheritdoc />
        public override IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();
            var configuration = ConfigurationProvider.Instance.GetConfiguration<RecordingConfiguration>();
            var recordingOptions = Mapping.Mapper.Map<RecordingOptions>(configuration);
            return new Recorder(recordingOptions, GetFfmpegPath(), RecordingHelper.GetDefaultVideoFolder());
        }

        /// <inheritdoc />
        public override RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFfmpegPath());

            var recordingDevices = new RecordingDevices();

            cli.Run("-list_devices true -f dshow -i dummy");
            cli.WaitForExit();

            string output = cli.Output.ToString();
            bool isVideo = true;
            Regex regex = new Regex(@"\[dshow @ \w+\]  ""(.+)""", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            var lines = ArgumentsHelper.SplitLines(output);
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
