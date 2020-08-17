using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Eto.Forms;
using RedShot.Helpers;
using RedShot.Helpers.CLI;
using RedShot.Helpers.Ffmpeg;
using RedShot.Helpers.Ffmpeg.Devices;

namespace RedShot.Recording.Recorders.Linux
{
    public class LinuxRecordingManager : IRecordingManager
    {
        private readonly string ffmpegName;

        private readonly CliManager simpleCliManager;

        public LinuxRecordingManager()
        {
            simpleCliManager = new CliManager();
            ffmpegName = "ffmpeg";
        }

        public IRecorder GetRecorder(FFmpegOptions options)
        {
            ThrowIfNotFoundFfmpegBinary();

            return new LinuxRecorder(options);
        }

        public bool CheckFFmpeg()
        {
            simpleCliManager.Run(ffmpegName);
            return simpleCliManager.Output.ToString().Contains("ffmpeg version");
        }

        public RecordingDevices GetRecordingDevices()
        {
            ThrowIfNotFoundFfmpegBinary();

            //var cli = new FFmpegCliManager(GetFullFfmpegPath());

            var devices = new RecordingDevices();

            //cli.Run("-list_devices true -f dshow -i dummy");

            //cli.WaitForExit();

            //string output = cli.Output.ToString();
            //string[] lines = GetLines(output);
            //bool isVideo = true;
            //Regex regex = new Regex(@"\[dshow @ \w+\]  ""(.+)""", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            //foreach (string line in lines)
            //{
            //    if (line.Contains("] DirectShow video devices", StringComparison.InvariantCulture))
            //    {
            //        isVideo = true;
            //        continue;
            //    }

            //    if (line.Contains("] DirectShow audio devices", StringComparison.InvariantCulture))
            //    {
            //        isVideo = false;
            //        continue;
            //    }

            //    Match match = regex.Match(line);

            //    if (match.Success)
            //    {
            //        string value = match.Groups[1].Value;

            //        if (isVideo)
            //        {
            //            devices.VideoDevices.Add(new Device(value, value));
            //        }
            //        else
            //        {
            //            devices.AudioDevices.Add(new Device(value, value));
            //        }
            //    }
            //}

            return devices;
        }

        public void InstallFFmpeg()
        {
            if (CheckFFmpeg())
            {
                throw new Exception("FFmpeg is installed already!");
            }

            MessageBox.Show("Download ffmpeg package to your system before recording video", MessageBoxButtons.OK, MessageBoxType.Information);
        }

        private void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new DllNotFoundException($"ffmpeg binary is not found!");
            }
        }

        private string[] GetLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }
    }
}
