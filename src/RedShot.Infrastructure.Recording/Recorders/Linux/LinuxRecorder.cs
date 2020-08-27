﻿using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
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
