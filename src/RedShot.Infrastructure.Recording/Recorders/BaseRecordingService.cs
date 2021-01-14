﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Common.Notifying;

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

            var downloader = new Downloader();
            var downloadForm = new DownloadForm(downloader, "FFmpeg Downloader");
            downloadForm.Closed += (o, e) =>
            {
                downloader.CancelAsync();
                downloader.Dispose();
            };
            downloadForm.Show();

            downloader.DownloadAsync(BinariesUrl, "ffmpeg.zip", (path) =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(path, GetFfmpegPath());
                    NotifyHelper.Notify("FFmpeg has been downloaded", "RedShot", NotifyStatus.Success);
                }
                catch (Exception e)
                {
                    var errorMessage = "An error occurred while FFmpeg was installing";

                    logger.Error(e, $"{errorMessage}.");
                    MessageBox.Show($"{errorMessage}: {e.Message}", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            });
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
        /// Get lines.
        /// </summary>
        protected string[] GetLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Get full FFmpeg path.
        /// </summary>
        protected virtual string GetFullFfmpegPath()
        {
            return GetFfmpegFiles().FirstOrDefault();
        }

        /// <summary>
        /// Get FFmpeg files.
        /// </summary>
        protected virtual string[] GetFfmpegFiles()
        {
            return Directory.GetFiles(GetFfmpegPath(), FfmpegBinaryName, SearchOption.AllDirectories);
        }

        /// <summary>
        /// Get FFmpeg path.
        /// </summary>
        protected virtual string GetFfmpegPath()
        {
            return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFmpeg")).FullName;
        }
    }
}
