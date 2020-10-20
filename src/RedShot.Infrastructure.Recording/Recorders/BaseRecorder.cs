using System;
using System.IO;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Formatting;

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
        public string VideoFolderPath { get; }

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
        protected readonly FFmpegOptions options;

        /// <summary>
        /// CLI manager.
        /// </summary>
        protected readonly FFmpegCliManager cliManager;

        /// <summary>
        /// Initialize.
        /// </summary>
        public BaseRecorder(FFmpegOptions options, string ffmpegPath, string videoFolderPath = null)
        {
            this.options = options;
            cliManager = new FFmpegCliManager(ffmpegPath);

            if (string.IsNullOrEmpty(videoFolderPath))
            {
                VideoFolderPath = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "RedShot")).FullName;
            }
            else
            {
                VideoFolderPath = videoFolderPath;
            }
        }

        /// <inheritdoc />
        public virtual void Start(Rectangle area)
        {
            var deviceArgs = GetDeviceArgs(area);
            var pathName = $"RedShot-Video-{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
            var path = Path.Combine(VideoFolderPath, $"{pathName}.{options.Extension}");
            var name = FormatManager.GetFormattedName();

            LastVideo = new VideoFile(name, path);
            var outputArgs = FFmpegArgsHelper.GetArgsForOutput(path);

            cliManager.Run($"-thread_queue_size 1024 {deviceArgs} {options.GetFFmpegArgs()} {outputArgs}");
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
