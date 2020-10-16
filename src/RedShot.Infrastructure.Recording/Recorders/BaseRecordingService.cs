using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure.Recording.Recorders
{
    internal abstract class BaseRecordingService : IRecordingService
    {
        protected abstract string FfmpegBinaryName { get; }

        protected abstract string BinariesUrl { get; }

        protected readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public abstract IRecorder GetRecorder();

        /// <inheritdoc />
        public bool CheckFFmpeg()
        {
            return GetFfmpegFiles().Any();
        }

        /// <inheritdoc />
        public abstract IRecordingDevices GetRecordingDevices();

        /// <inheritdoc />
        public virtual void InstallFFmpeg()
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

        protected virtual void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new FileNotFoundException($"FFmpeg binary is not found!", FfmpegBinaryName);
            }
        }

        protected string[] GetLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        protected virtual string GetFullFfmpegPath()
        {
            return GetFfmpegFiles().First();
        }

        protected virtual string[] GetFfmpegFiles()
        {
            return Directory.GetFiles(GetFfmpegPath(), FfmpegBinaryName, SearchOption.AllDirectories);
        }

        protected virtual string GetFfmpegPath()
        {
            return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
        }
    }
}
