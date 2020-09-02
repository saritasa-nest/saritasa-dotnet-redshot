using System;
using System.IO;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.Formatting;

namespace RedShot.Infrastructure.Recording.Recorders
{
    internal abstract class BaseRecorder : IRecorder
    {
        public string VideoFolderPath { get; }

        public VideoFile LastVideo { get; protected set; }

        public bool IsRecording
        {
            get
            {
                return cliManager.IsRecording;
            }
        }

        protected readonly FFmpegOptions options;
        protected readonly FFmpegCliManager cliManager;

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

        protected abstract string GetDeviceArgs(Rectangle captureArea);

        public virtual IFile GetVideo()
        {
            Stop();
            return LastVideo;
        }
    }
}
