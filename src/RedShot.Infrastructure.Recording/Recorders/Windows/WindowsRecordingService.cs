using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;
using RedShot.Infrastructure.Recording;

namespace RedShot.Recording.Recorders.Windows
{
    /// <summary>
    /// Windows recorder service.
    /// </summary>
    internal class WindowsRecordingService : IRecordingService
    {
        private const string BinariesUrl = "https://github.com/BtbN/FFmpeg-Builds/releases/download/autobuild-2020-10-11-12-31/ffmpeg-N-99531-g2be3eb7f77-win64-gpl.zip";
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ffmpegBinaryName;

        /// <summary>
        /// Initializes Windows recording service.
        /// </summary>
        public WindowsRecordingService()
        {
            ffmpegBinaryName = "ffmpeg.exe";
        }

        /// <inheritdoc />
        public IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();

            var options = ConfigurationManager.GetSection<FFmpegConfiguration>().Options;

            return new WindowsRecorder(options, GetFullFfmpegPath());
        }

        /// <inheritdoc />
        public bool CheckFFmpeg()
        {
            return GetFfmpegFiles().Any();
        }

        /// <inheritdoc />
        public IRecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFullFfmpegPath());

            var videoDevices = new List<Device>();
            var audioDevices = new List<Device>();

            cli.Run("-list_devices true -f dshow -i dummy");

            cli.WaitForExit();

            string output = cli.Output.ToString();
            string[] lines = GetLines(output);
            bool isVideo = true;
            Regex regex = new Regex(@"\[dshow @ \w+\]  ""(.+)""", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            foreach (string line in lines)
            {
                if (line.Contains("] DirectShow video devices", StringComparison.InvariantCulture))
                {
                    isVideo = true;
                    continue;
                }

                if (line.Contains("] DirectShow audio devices", StringComparison.InvariantCulture))
                {
                    isVideo = false;
                    continue;
                }

                Match match = regex.Match(line);

                if (match.Success)
                {
                    string value = match.Groups[1].Value;

                    if (isVideo)
                    {
                        videoDevices.Add(new Device(value, value));
                    }
                    else
                    {
                        audioDevices.Add(new Device(value, value));
                    }
                }
            }

            return new RecordingDevices(videoDevices, audioDevices);
        }

        /// <inheritdoc />
        public void InstallFFmpeg()
        {
            if (CheckFFmpeg())
            {
                throw new Exception("FFmpeg is installed already!");
            }

            using (var yesNoDialog = new YesNoDialog())
            {
                yesNoDialog.Size = new Size(400, 200);
                yesNoDialog.Message = "FFmpeg is not installed. Do you want to automatically install it?";
                yesNoDialog.Title = "FFmpeg installing";

                if (yesNoDialog.ShowModal() != DialogResult.Yes)
                {
                    return;
                }
            }

            var downloader = new Downloader();
            downloader.DownloadAsync(BinariesUrl, "ffmpeg.zip", (path) =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(path, GetFfmpegPath());
                    NotifyHelper.Notify("FFmpeg has been downloaded", "RedShot", NotifyStatus.Success);
                    downloader.Dispose();
                }
                catch (Exception e)
                {
                    var errorMessage = "An error occurred while FFmpeg was installing";

                    logger.Error(e, $"{errorMessage}.");
                    MessageBox.Show($"{errorMessage}: {e.Message}", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            });
        }

        private void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new FileNotFoundException($"FFmpeg binary is not found!", ffmpegBinaryName);
            }
        }

        private string GetFullFfmpegPath()
        {
            return GetFfmpegFiles().First();
        }

        private string[] GetFfmpegFiles()
        {
            return Directory.GetFiles(GetFfmpegPath(), ffmpegBinaryName, SearchOption.AllDirectories);
        }

        private string[] GetLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        private string GetFfmpegPath()
        {
            return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
        }
    }
}
