using System;
using System.IO;
using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.Recording;

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

        public virtual void Start(Rectangle area)
        {
            var deviceArgs = GetDeviceArgs(area);
            var optionsArgs = FFmpegArgsHelper.GetFFmpegArgsFromOptions(options);

            var name = DateTime.Now.ToFileTime();

            var path = Path.Combine(VideoFolderPath, $"{name}.{options.Extension}");

            LastVideo = new VideoFile(name.ToString(), path);

            var outputArgs = FFmpegArgsHelper.GetArgsForOutput(path);

            cliManager.Run($"-thread_queue_size 1024 {deviceArgs} {optionsArgs} {outputArgs}");
        }

        public virtual void Stop()
        {
            cliManager.Stop();
        }

        protected virtual string GetDeviceArgs(Rectangle captureArea)
        {
            if (captureArea.Width % 2 != 0)
            {
                captureArea.Width--;
            }

            if (captureArea.Height % 2 != 0)
            {
                captureArea.Height--;
            }

            var args = new StringBuilder();

            if (options.UseGdigrab || options.VideoDevice == null)
            {
                args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -framerate {options.Fps} -f x11grab -i :0.0+{captureArea.Location.X},{captureArea.Location.Y}");
                args.Append($" -draw_mouse {(options.DrawCursor ? '1' : '0')} ");
            }

            if (options.UseMicrophone && options.AudioDevice != null)
            {
                if (options.AudioDevice != null)
                {
                    args.Append($"-f alsa -ac 2 -i {options.AudioDevice.CompatibleFfmpegName}");
                }
                else
                {
                    args.Append($"-f alsa -ac 2 -i hw:0 ");
                }
            }

            return args.ToString();
        }

        public virtual IFile GetVideo()
        {
            Stop();
            return LastVideo;
        }
    }
}
