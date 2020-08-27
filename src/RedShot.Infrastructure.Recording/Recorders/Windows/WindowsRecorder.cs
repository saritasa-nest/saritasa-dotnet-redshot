using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.Recording.Recorders;

namespace RedShot.Recording.Recorders.Windows
{
    /// <summary>
    /// Recorder for Windows OS.
    /// </summary>
    internal class WindowsRecorder : BaseRecorder
    {
        /// <summary>
        /// Initializes Windows recorder.
        /// </summary>
        public WindowsRecorder(FFmpegOptions options, string ffmpegPath, string videoFolderPath = null) : base(options, ffmpegPath, videoFolderPath)
        {
        }

        /// <inheritdoc/>
        protected override string GetDeviceArgs(Rectangle captureArea)
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
    }
}
