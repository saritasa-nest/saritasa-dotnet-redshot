﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using RedShot.Helpers;
using RedShot.Helpers.Ffmpeg;
using RedShot.Helpers.Ffmpeg.Devices;

namespace RedShot.Recording.Recorders.Windows
{
    public class WindowsRecordingManager : IRecordingManager
    {
        private readonly string ffmpegPath;

        private string GetFullFfmpegPath()
        {
            return Directory.GetFiles(ffmpegPath, "ffmpeg.exe", SearchOption.AllDirectories).First();
        }

        public bool IsFFmpegDetected { get; private set; }

        public WindowsRecordingManager()
        {
            ffmpegPath = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
        }

        public IRecorder GetRecorder(FFmpegOptions options)
        {
            ThrowIfNotFoundFfmpegBinary();

            return new WindowsRecorder(options, GetFullFfmpegPath());
        }

        public bool CheckFFmpeg()
        {
            return Directory.GetFiles(ffmpegPath, "ffmpeg.exe", SearchOption.AllDirectories).Any();
        }

        public RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            var cli = new FFmpegCliManager(GetFullFfmpegPath());

            var devices = new RecordingDevices();

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
                        devices.VideoDevices.Add(new Device(value, value));
                    }
                    else
                    {
                        devices.AudioDevices.Add(new Device(value, value));
                    }
                }
            }

            return devices;
        }

        public void InstallFFmpeg()
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

            using var downloader = new Downloader();
            var path = downloader.Download(url, ffmpegZipName);

            ZipFile.ExtractToDirectory(path, ffmpegPath);
        }

        private void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new FileNotFoundException($"ffmpeg.exe is not found!");
            }
        }

        private string[] GetLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }
    }
}
