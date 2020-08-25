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
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Devices;
using RedShot.Infrastructure.Recording;

namespace RedShot.Recording.Recorders.Windows
{
    public class WindowsRecordingService : IRecordingService
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ffmpegPath;

        private readonly string ffmpegBinaryName;

        public WindowsRecordingService()
        {
            ffmpegPath = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
            ffmpegBinaryName = "ffmpeg.exe";
        }

        public IRecorder GetRecorder()
        {
            ThrowIfNotFoundFfmpegBinary();

            var options = ConfigurationManager.GetSection<FFmpegConfiguration>().Options;

            return new WindowsRecorder(options, GetFullFfmpegPath());
        }

        public bool CheckFFmpeg()
        {
            return GetFfmpegFiles().Any();
        }

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

        public bool InstallFFmpeg()
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
                    return false;
                }
            }

            string url;

            if (Environment.Is64BitOperatingSystem)
            {
                url = "https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-latest-win64-static.zip";
            }
            else
            {
                url = "https://ffmpeg.zeranoe.com/builds/win32/static/ffmpeg-latest-win32-static.zip";
            }

            var ffmpegZipName = "ffmpeg.zip";

            try
            {
                using var downloader = new Downloader();

                downloader.DownloadAsync(url, ffmpegZipName, (path) => 
                {
                    ZipFile.ExtractToDirectory(path, ffmpegPath);
                    RecordingManager.InitiateRecording();
                });

                //ZipFile.ExtractToDirectory(path, ffmpegPath);

                return false;
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while FFmpeg was installing.", e);
                MessageBox.Show($"An error occurred while FFmpeg was installing: {e.Message}", MessageBoxButtons.OK, MessageBoxType.Error);

                return false;
            }
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
            return Directory.GetFiles(ffmpegPath, ffmpegBinaryName, SearchOption.AllDirectories);
        }

        private string[] GetLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }
    }
}
