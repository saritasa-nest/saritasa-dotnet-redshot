using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using RedShot.Infrastructure.Recording.Common.Recorders;

namespace RedShot.Infrastructure.Recording.Unix.Linux
{
    /// <summary>
    /// Recorder for Linux OS.
    /// </summary>
    internal class LinuxRecorder : BaseRecorder
    {
        /// <summary>
        /// Initializes Linux recorder.
        /// </summary>
        public LinuxRecorder(FFmpegConfiguration configuration, string videoFolderPath) : base(configuration, "ffmpeg", videoFolderPath)
        {
        }

        /// <inheritdoc/>
        protected override string GetDeviceArgs(Rectangle captureArea)
        {
            var args = new StringBuilder();

            if (ffmpegOptions.UseGdigrab || ffmpegOptions.VideoDevice == null)
            {
                args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -framerate {ffmpegOptions.Fps} -f x11grab -i :0.0+{captureArea.Location.X},{captureArea.Location.Y}");
                args.Append($" -draw_mouse {(ffmpegOptions.DrawCursor ? '1' : '0')} ");
            }

            if (audioOptions.RecordAudio)
            {
                args.Append($"-f alsa -ac 2 -i hw:0 ");
            }

            return args.ToString();
        }
    }
}
