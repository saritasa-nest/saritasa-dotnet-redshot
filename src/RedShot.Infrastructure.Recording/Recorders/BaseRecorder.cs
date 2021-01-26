using System;
using System.IO;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Recording.Abstractions;
using System.Text;
using System.Linq;

namespace RedShot.Infrastructure.Recording.Recorders
{
    /// <summary>
    /// Base recorder.
    /// </summary>
    internal abstract class BaseRecorder : IRecorder
    {
        /// <summary>
        /// Video folder path.
        /// </summary>
        protected readonly string videoFolderPath;

        /// <summary>
        /// Last video.
        /// </summary>
        public VideoFile LastVideo { get; protected set; }

        /// <summary>
        /// Is recording.
        /// </summary>
        public bool IsRecording
        {
            get
            {
                return cliManager.IsRecording;
            }
        }

        /// <summary>
        /// FFmpeg options.
        /// </summary>
        protected readonly FFmpegOptions ffmpegOptions;

        /// <summary>
        /// Audio options.
        /// </summary>
        protected readonly AudioOptions audioOptions;

        /// <summary>
        /// CLI manager.
        /// </summary>
        protected readonly FFmpegCliManager cliManager;

        /// <summary>
        /// Initialize.
        /// </summary>
        public BaseRecorder(FFmpegConfiguration configuration, string ffmpegPath, string videoFolderPath)
        {
            this.videoFolderPath = videoFolderPath;
            ffmpegOptions = configuration.FFmpegOptions;
            audioOptions = configuration.AudioOptions;
            cliManager = new FFmpegCliManager(ffmpegPath);
        }

        /// <inheritdoc />
        public virtual void Start(Rectangle area)
        {
            var deviceArgs = GetDeviceArgs(area);
            var pathName = $"RedShot-Video-{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
            var path = Path.Combine(videoFolderPath, $"{pathName}.{ffmpegOptions.Extension}");
            var name = FormatManager.GetFormattedName();

            LastVideo = new VideoFile(name, path);
            var outputArgs = FFmpegArgsHelper.GetArgsForOutput(path);

            cliManager.Run($"-thread_queue_size 1024 {deviceArgs} {ffmpegOptions.GetFFmpegArgs()} {GetAudioStreamArgs()} {outputArgs}");
        }

        /// <summary>
        /// Get audio stream arguments.
        /// </summary>
        protected virtual string GetAudioStreamArgs()
        {
            var audioArgsBuilder = new StringBuilder();

            if (audioOptions.RecordAudio)
            {
                if (audioOptions.Devices.Any())
                {
                    var count = audioOptions.Devices.Count();

                    if (count > 1)
                    {
                        audioArgsBuilder.Append("-filter_complex \"");
                        for (int i = 0; i < count; i++)
                        {
                            audioArgsBuilder.Append($"[{i}:a]");
                        }
                        audioArgsBuilder.Append($"amerge=inputs={count}[a]\" -map 2 -map \"[a]\"");
                    }
                }
            }

            return audioArgsBuilder.ToString();
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            cliManager.Stop();
        }

        /// <summary>
        /// Get last video.
        /// </summary>
        public virtual IFile GetVideo()
        {
            Stop();
            return LastVideo;
        }

        /// <summary>
        /// Get device arguments.
        /// </summary>
        protected abstract string GetDeviceArgs(Rectangle captureArea);
    }
}
