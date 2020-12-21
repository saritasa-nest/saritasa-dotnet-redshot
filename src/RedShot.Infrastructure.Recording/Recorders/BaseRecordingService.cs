using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Recording.Abstractions;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

namespace RedShot.Infrastructure.Recording.Recorders
{
    /// <summary>
    /// Base recording service.
    /// </summary>
    internal abstract class BaseRecordingService : IRecordingService
    {
        /// <summary>
        /// FFmpeg binary name.
        /// </summary>
        protected abstract string FfmpegBinaryName { get; }

        /// <summary>
        /// Binaries URL.
        /// </summary>
        protected abstract string BinariesUrl { get; }

        /// <summary>
        /// Logger.
        /// </summary>
        protected readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public abstract IRecorder GetRecorder();

        /// <inheritdoc />
        public bool CheckFFmpeg()
        {
            return File.Exists(GetFfmpegPath());
        }

        /// <inheritdoc />
        public abstract RecordingDevices GetRecordingDevices();

        /// <inheritdoc />
        public virtual void InstallFFmpeg()
        {
            if (CheckFFmpeg())
            {
                throw new Exception("FFmpeg is installed already!");
            }

            const string message = "FFmpeg is not installed. Do you want to automatically install it?";
            const string title = "FFmpeg Installing";
            var yesNoDialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxType.Warning);
            if (yesNoDialogResult != DialogResult.Yes)
            {
                return;
            }

            var downloader = new Downloader();
            downloader.DownloadAsync(BinariesUrl, "ffmpeg.zip", (path) =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(path, GetFfmpegBinariesFolder());
                    NotifyHelper.Notify("FFmpeg has been downloaded", "RedShot", NotifyStatus.Success);
                    downloader.Dispose();
                }
                catch (Exception e)
                {
                    var errorMessage = "An error occurred while FFmpeg was installing";

                    logger.Error(e, $"{errorMessage}.");
                    MessageBox.Show($"{errorMessage}: {e.Message}", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            },
            "Downloading FFmpeg");
        }

        /// <summary>
        /// Throw if not found FFmpeg binary.
        /// </summary>
        protected virtual void ThrowIfNotFoundFfmpegBinary()
        {
            if (!CheckFFmpeg())
            {
                throw new FileNotFoundException($"FFmpeg binary is not found!", FfmpegBinaryName);
            }
        }

        /// <summary>
        /// Get full FFmpeg path.
        /// </summary>
        protected virtual string GetFfmpegPath()
        {
            return Directory.GetFiles(GetFfmpegBinariesFolder(), FfmpegBinaryName, SearchOption.AllDirectories).First();
        }

        /// <summary>
        /// Get FFmpeg path.
        /// </summary>
        protected virtual string GetFfmpegBinariesFolder()
        {
            return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
        }
    }
}
