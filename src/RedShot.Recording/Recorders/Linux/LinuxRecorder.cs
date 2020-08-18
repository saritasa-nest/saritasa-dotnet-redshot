using System;
using System.IO;
using System.Text;
using Eto.Drawing;
using RedShot.Helpers.Ffmpeg;
using RedShot.Helpers.Ffmpeg.Options;

namespace RedShot.Recording.Recorders.Linux
{
    public class LinuxRecorder : IRecorder
    {
        public string VideoFolderPath { get; }

        public string LastVideoPath { get; private set; }

        public bool IsRecording
        {
            get
            {
                return cliManager.IsProcessRunning;
            }
        }

        private readonly FFmpegOptions options;
        private readonly FFmpegCliManager cliManager;

        public LinuxRecorder(FFmpegOptions options, string videoFolderPath = null)
        {
            this.options = options;
            cliManager = new FFmpegCliManager("ffmpeg");

            if (string.IsNullOrEmpty(videoFolderPath))
            {
                VideoFolderPath = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "RedShot")).FullName;
            }
            else
            {
                VideoFolderPath = videoFolderPath;
            }
        }

        public void Start(Rectangle area)
        {
            var deviceArgs = GetLinuxDeviceArgs(area);
            var optionsArgs = FFmpegArgsHelper.GetFFmpegArgsFromOptions(options);

            var name = DateTime.Now.ToFileTime();

            LastVideoPath = Path.Combine(VideoFolderPath, $"{name}.{options.Extension}");

            var outputArgs = FFmpegArgsHelper.GetArgsForOutput(LastVideoPath);

            cliManager.Run($"-thread_queue_size 1024 {deviceArgs} {optionsArgs} {outputArgs}");
        }

        public void Stop()
        {
            cliManager.Stop();
        }

        private string GetLinuxDeviceArgs(Rectangle captureArea)
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
    }
}
