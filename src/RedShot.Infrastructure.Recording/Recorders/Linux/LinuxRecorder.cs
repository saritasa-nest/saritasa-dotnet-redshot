using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Recording.Recorders;

namespace RedShot.Recording.Recorders.Linux
{
    /// <summary>
    /// Recorder for Linux OS.
    /// </summary>
    internal class LinuxRecorder : BaseRecorder
    {
        /// <summary>
        /// Initializes Linux recorder.
        /// </summary>
        public LinuxRecorder(FFmpegOptions options, string videoFolderPath = null) : base(options, "ffmpeg", videoFolderPath)
        {
        }

        /// <inheritdoc/>
        protected override string GetDeviceArgs(Rectangle captureArea)
        {
            var args = new StringBuilder();

            if (options.UseGdigrab || options.VideoDevice == null)
            {
                args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -framerate {options.Fps} -f x11grab -i :0.0+{captureArea.Location.X},{captureArea.Location.Y}");
                args.Append($" -draw_mouse {(options.DrawCursor ? '1' : '0')} ");
            }

            if (options.UseAudio)
            {
                args.Append($"-f alsa -ac 2 -i hw:0 ");
            }

            return args.ToString();
        }
    }
}
