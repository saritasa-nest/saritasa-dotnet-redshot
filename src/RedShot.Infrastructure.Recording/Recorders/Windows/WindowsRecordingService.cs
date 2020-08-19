﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Devices;
using RedShot.Infrastructure.Recording;

namespace RedShot.Recording.Recorders.Windows
{
    public class WindowsRecordingService : IRecordingService
    {
        private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ffmpegPath;

        private readonly string ffmpegBinaryName;

        public WindowsRecordingService()
        {
            ffmpegPath = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
            ffmpegBinaryName = "ffmpeg.exe";
        }

        public IRecorder GetRecorder(FFmpegOptions options)
        {
            ThrowIfNotFoundFfmpegBinary();

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

            string url;

            if (IntPtr.Size == 8)
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
                var path = downloader.Download(url, ffmpegZipName);

                ZipFile.ExtractToDirectory(path, ffmpegPath);

                return true;
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while FFmpeg was installing.", e);
                MessageBox.Show($"An error occurred while FFmpeg was installing: {e.Message}", MessageBoxButtons.OK, MessageBoxType.Error);

                return false;
            }
        }

        private void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new FileNotFoundException($"ffmpeg binary is not found!", ffmpegBinaryName);
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

        public IRecorder GetRecorder()
        {
            throw new NotImplementedException();
        }
    }
}
