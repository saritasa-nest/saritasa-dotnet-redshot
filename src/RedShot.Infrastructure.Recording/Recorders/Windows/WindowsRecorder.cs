using System;
using System.IO;
using System.Text;
using Eto.Drawing;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.Recording;
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

        public override void Start(Rectangle area)
        {
            var deviceArgs = GetDeviceArgs(area);
            var name = $"RedShot-Video-{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
            var path = Path.Combine(VideoFolderPath, $"{name}.{options.Extension}");
            LastVideo = new VideoFile(name.ToString(), path);
            var outputArgs = FFmpegArgsHelper.GetArgsForOutput(path);

            var mapArgs = string.Empty;

            if (options.UseAudio == true && options.PrimaryAudioDevice != null && options.OptionalAudioDevice != null)
            {
                mapArgs = "-filter_complex \"[0:a][1:a]amerge=inputs=2[a]\" -map 2 -map \"[a]\"";
            }

            cliManager.Run($"-thread_queue_size 1024 {deviceArgs} {options.GetFFmpegArgs()} {mapArgs} {outputArgs}");
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

            if (options.UseAudio)
            {
                if (options.PrimaryAudioDevice != null)
                {
                    args.AppendFormat("-f dshow -i audio=\"{0}\" ", options.PrimaryAudioDevice.CompatibleFfmpegName);
                }

                if (options.OptionalAudioDevice != null)
                {
                    if (options.PrimaryAudioDevice != options.OptionalAudioDevice)
                    {
                        args.AppendFormat("-f dshow -i audio=\"{0}\" ", options.OptionalAudioDevice.CompatibleFfmpegName);
                    }
                }
            }

            if (options.UseGdigrab || options.VideoDevice == null)
            {
                args.Append($"-f gdigrab -framerate {options.Fps} -offset_x {captureArea.Location.X} -offset_y {captureArea.Location.Y} ");
                args.Append($"-video_size {captureArea.Size.Width}x{captureArea.Size.Height} -draw_mouse {(options.DrawCursor ? '1' : '0')} -i desktop ");
            }
            else
            {
                args.AppendFormat($"-f dshow -framerate {options.Fps} -i video=\"{options.VideoDevice.CompatibleFfmpegName}\" ");
            }

            return args.ToString();
        }
    }
}
