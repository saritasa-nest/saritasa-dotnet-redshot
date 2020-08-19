using System;
using System.IO;
using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.Recording;

namespace RedShot.Recording.Recorders.Windows
{
    public class WindowsRecorder : IRecorder
    {
        public string VideoFolderPath { get; }

        public VideoFile LastVideo { get; private set; }

        public bool IsRecording
        {
            get
            {
                return cliManager.IsRecording;
            }
        }

        private readonly FFmpegOptions options;
        private readonly FFmpegCliManager cliManager;

        public WindowsRecorder(FFmpegOptions options, string ffmpegPath, string videoFolderPath = null)
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

        public void Start(Rectangle area)
        {
            var deviceArgs = GetWindowsDeviceArgs(area);
            var optionsArgs = FFmpegArgsHelper.GetFFmpegArgsFromOptions(options);

            var name = DateTime.Now.ToFileTime();

            var path = Path.Combine(VideoFolderPath, $"{name}.{options.Extension}");

            LastVideo = new VideoFile(name.ToString(), path);

            var outputArgs = FFmpegArgsHelper.GetArgsForOutput(path);

            cliManager.Run($"-thread_queue_size 1024 {deviceArgs} {optionsArgs} {outputArgs}");
        }

        public void Stop()
        {
            cliManager.Stop();
        }

        private string GetWindowsDeviceArgs(Rectangle captureArea)
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
                args.Append($"-f gdigrab -framerate {options.Fps} -offset_x {captureArea.Location.X} -offset_y {captureArea.Location.Y} ");
                args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -draw_mouse {(options.DrawCursor ? '1' : '0')} -i desktop ");
            }
            else
            {
                args.AppendFormat($"-f dshow -framerate {options.Fps} -i video=\"{options.VideoDevice.CompatibleFfmpegName}\" ");
            }

            if (options.UseMicrophone && options.AudioDevice != null)
            {
                args.AppendFormat("-f dshow -i audio=\"{0}\" ", options.AudioDevice.CompatibleFfmpegName);
            }

            return args.ToString();
        }

        public IFile GetVideo()
        {
            Stop();
            return LastVideo;
        }
    }
}
