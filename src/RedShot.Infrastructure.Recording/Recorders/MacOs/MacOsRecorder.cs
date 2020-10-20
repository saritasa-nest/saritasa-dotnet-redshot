using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Infrastructure.Recording.Recorders.MacOs
{
    internal class MacOsRecorder : BaseRecorder
    {
        /// <summary>
        /// Initializes Linux recorder.
        /// </summary>
        public MacOsRecorder(FFmpegOptions options, string videoFolderPath = null) : base(options, "ffmpeg", videoFolderPath)
        {
        }

        /// <inheritdoc/>
        protected override string GetDeviceArgs(Rectangle captureArea)
        {
            var args = new StringBuilder();

            args.Append($" -framerate {options.Fps} -f avfoundation -i \"default:none\" ");
            args.Append($" -draw_mouse {(options.DrawCursor ? '1' : '0')} ");

            if (options.VideoDevice != null)
            {
                args.AppendFormat("-video_device_index {0} ", options.VideoDevice.CompatibleFfmpegName);
            }

            if (options.UseAudio)
            {
                if (options.PrimaryAudioDevice != null)
                {
                    args.AppendFormat("-audio_device_index {0} ", options.PrimaryAudioDevice.CompatibleFfmpegName);
                }

                if (options.OptionalAudioDevice != null)
                {
                    if (options.PrimaryAudioDevice != options.OptionalAudioDevice)
                    {
                        args.AppendFormat("-audio_device_index {0} ", options.PrimaryAudioDevice.CompatibleFfmpegName);
                    }
                }
            }

            args.Append($"-vf  \"crop = {captureArea.Size.Width}:{captureArea.Size.Height}:{captureArea.Location.X}:{captureArea.Location.Y}\" ");

            return args.ToString();
        }
    }
}
