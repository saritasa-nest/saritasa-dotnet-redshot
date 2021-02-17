using System.Diagnostics;
using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using RedShot.Infrastructure.Recording.Common.Recorders;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// Recorder for Windows OS.
    /// </summary>
    internal class Recorder : BaseRecorder
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Recorder(FFmpegConfiguration options, string ffmpegPath, string videoFolderPath) : base(options, ffmpegPath, videoFolderPath)
        {
            cliManager = new FFmpegCliManager(ffmpegPath, SetProcessTracker);
        }

        private static void SetProcessTracker(Process process)
        {
            Platforms.Windows.ChildProcessTracking.ChildProcessTracker.AddProcess(process);
        }

        /// <inheritdoc/>
        protected override string GetDeviceArgs(Rectangle captureArea)
        {
            var args = new StringBuilder();

            if (audioOptions.RecordAudio)
            {
                foreach (var device in audioOptions.Devices)
                {
                    args.AppendFormat("-f dshow -i audio=\"{0}\" ", device.CompatibleFfmpegName);
                }
            }

            if (ffmpegOptions.UseGdigrab || ffmpegOptions.VideoDevice == null)
            {
                args.Append($"-f gdigrab -framerate {ffmpegOptions.Fps} -offset_x {captureArea.Location.X} -offset_y {captureArea.Location.Y} ");
                args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -draw_mouse {(ffmpegOptions.DrawCursor ? '1' : '0')} -i desktop ");
            }
            else
            {
                args.AppendFormat($"-f dshow -framerate {ffmpegOptions.Fps} -i video=\"{ffmpegOptions.VideoDevice.CompatibleFfmpegName}\" ");
            }

            return args.ToString();
        }
    }
}
