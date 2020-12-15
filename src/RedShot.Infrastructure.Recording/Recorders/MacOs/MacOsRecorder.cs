using System.Text;
using Eto.Drawing;

namespace RedShot.Infrastructure.Recording.Recorders.MacOs
{
    internal class MacOsRecorder : BaseRecorder
    {
        /// <summary>
        /// Initializes Linux recorder.
        /// </summary>
        public MacOsRecorder(FFmpegConfiguration options, string ffmpegPath, string videoFolderPath = null) : base(options, ffmpegPath, videoFolderPath)
        {
        }

        /// <inheritdoc/>
        protected override string GetDeviceArgs(Rectangle captureArea)
        {
            var args = new StringBuilder();

            args.Append($" -framerate {ffmpegOptions.Fps} -f avfoundation -i \"default:none\" ");
            args.Append($" -draw_mouse {(ffmpegOptions.DrawCursor ? '1' : '0')} ");

            if (ffmpegOptions.VideoDevice != null)
            {
                args.AppendFormat("-video_device_index {0} ", ffmpegOptions.VideoDevice.CompatibleFfmpegName);
            }

            if (audioOptions.RecordAudio)
            {
                foreach (var device in audioOptions.Devices)
                {
                    args.AppendFormat("-audio_device_index {0} ", device.CompatibleFfmpegName);
                }
            }

            args.Append($"-vf  \"crop = {captureArea.Size.Width}:{captureArea.Size.Height}:{captureArea.Location.X}:{captureArea.Location.Y}\" ");

            return args.ToString();
        }
    }
}
