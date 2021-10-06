using Microsoft.Extensions.Options;
using RedShot.Infrastructure.Abstractions.Interfaces.Recording;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.DomainServices.Common.Services;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Recording.Common
{
    /// <summary>
    /// Recording service.
    /// </summary>
    internal class WindowsRecordingService : BaseRecordingService
    {
        public WindowsRecordingService(
            IOptionsMonitor<RecordingOptions> optionsMonitor,
            IRecordingFoldersService foldersService,
            IRecordingAppInstaller recordingApplication) : base(optionsMonitor, foldersService, recordingApplication)
        {
        }

        protected override string GenerateDeviceArguments(RecordingOptions recordingOptions, Rectangle recordingArea)
        {
            var args = new StringBuilder();

            var audioOptions = recordingOptions.AudioOptions;
            var ffmpegOptions = recordingOptions.FFmpegOptions;

            if (audioOptions.RecordAudio)
            {
                foreach (var device in audioOptions.Devices)
                {
                    args.AppendFormat("-f dshow -i audio=\"{0}\" ", device.CompatibleFfmpegName);
                }
            }

            if (ffmpegOptions.UseGdigrab || ffmpegOptions.VideoDevice == null)
            {
                args.Append($"-f gdigrab -framerate {ffmpegOptions.Fps} -offset_x {recordingArea.Location.X} -offset_y {recordingArea.Location.Y} ");
                args.Append($"-video_size {recordingArea.Size.Width}x{recordingArea.Size.Height} -draw_mouse {(ffmpegOptions.DrawCursor ? '1' : '0')} -i desktop ");
            }
            else
            {
                args.AppendFormat($"-f dshow -framerate {ffmpegOptions.Fps} -i video=\"{ffmpegOptions.VideoDevice.CompatibleFfmpegName}\" ");
            }

            return args.ToString();
        }
    }
}
