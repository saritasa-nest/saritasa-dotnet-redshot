using System;
using System.IO;
using System.Text;
using Eto.Drawing;
using RedShot.Recording.Helpers;

namespace RedShot.Recording.Recorders.Windows
{
    public class WindowsRecorder : IRecorder
    {
        public string VideoFolderPath { get; }

        public bool IsRecording
        {
            get
            {
                return cliManager.IsProcessRunning;
            }
        }

        private readonly FFmpegOptions options;
        private readonly FFmpegCliManager cliManager;

        public WindowsRecorder(FFmpegOptions options, string ffmpegPath)
        {
            this.options = options;
            cliManager = new FFmpegCliManager(ffmpegPath);

            VideoFolderPath = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "RedShot")).FullName;
        }

        public void Start(Rectangle area)
        {
            var deviceArgs = GetWindowsDeviceArgs(area, options);
            var optionsArgs = FFmpegArgsManager.GetFFmpegArgsFromOptions(options);

            var output = FFmpegArgsManager.GetArgsForOutput(VideoFolderPath, options);

            cliManager.Run($"{deviceArgs} {optionsArgs} {output}");
        }

        public void Stop()
        {
            if (IsRecording)
            {
                cliManager.Stop();
            }
        }

        private string GetWindowsDeviceArgs(Rectangle captureArea, FFmpegOptions options)
        {
            var args = new StringBuilder();

            args.Append($"-f gdigrab -framerate {options.Fps} -offset_x {captureArea.Location.X} -offset_y {captureArea.Location.Y} ");
            args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -draw_mouse {(options.DrawCursor ? '1' : '0')} -i desktop ");

            if (options.AudioDevice != null)
            {
                args.AppendFormat("-f dshow -i audio=\"{0}\" ", options.AudioDevice.CompatibleFfmpegName);
            }

            return args.ToString();
        }
    }
}
