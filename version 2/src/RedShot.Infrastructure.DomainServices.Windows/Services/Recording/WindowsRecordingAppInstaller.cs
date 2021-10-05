using Microsoft.Extensions.Options;
using RedShot.Infrastructure.DomainServices.Common.Configurations;
using Saritasa.Tools.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Recording
{
    public class WindowsRecordingAppInstaller : IRecordingAppInstaller
    {
        private const string FfmpegBinaryName = "ffmpeg.exe";

        private readonly FfmpegConfiguration ffmpegConfiguration;
        private readonly IRecordingFoldersService recordingFolders;
        private readonly IHttpClientFactory clientFactory;

        public WindowsRecordingAppInstaller(
            IOptions<FfmpegConfiguration> ffmpegConfiguration,
            IRecordingFoldersService recordingFolders,
            IHttpClientFactory clientFactory)
        {
            this.ffmpegConfiguration = ffmpegConfiguration.Value;
            this.recordingFolders = recordingFolders;
            this.clientFactory = clientFactory;
        }

        public async Task InstallAsync()
        {
            var tempDirectory = Path.GetTempPath();
            var downloadDirectory = Directory.CreateDirectory(
                Path.Combine(
                    tempDirectory, 
                    Guid.NewGuid().ToString()
                    )
                ).FullName;

            try
            {
                var zipFileTempPath = await DownloadFfmpegZipArchiveAsync(downloadDirectory);
                var binariesFolder = recordingFolders.GetRecordingAppBinariesFolder();
                ZipFile.ExtractToDirectory(zipFileTempPath, binariesFolder);
            }
            catch (Exception e)
            {
                throw new DomainException("An error occurred during recorder installation.", e);
            }
            finally
            {
                Directory.Delete(downloadDirectory, recursive: true);
            }
        }

        public Task<string> GetRecorderFileNameAsync()
        {
            var ffmpegBinary = GetRecorderBinary();

            if (ffmpegBinary == null)
            {
                throw new DomainException("The recorder are not installed.");
            }

            return Task.FromResult(ffmpegBinary);
        }

        public Task<bool> CheckRecorderExistingAsync()
        {
            var ffmpegBinary = GetRecorderBinary();
            return Task.FromResult(ffmpegBinary == null);
        }

        private string GetRecorderBinary()
        {
            var binariesFolder = recordingFolders.GetRecordingAppBinariesFolder();

            var ffmpegBinary = Directory.GetFiles(
                binariesFolder,
                FfmpegBinaryName,
                SearchOption.AllDirectories)
                .FirstOrDefault();

            return ffmpegBinary;
        }

        private async Task<string> DownloadFfmpegZipArchiveAsync(string downloadDirectory)
        {
            var zipFileTempPath = Path.Combine(
                downloadDirectory,
                ".ffmpeg");

            using var client = clientFactory.CreateClient();
            await using var stream = await client.GetStreamAsync(ffmpegConfiguration.WindowsBinariesUrl);
            await using var fileStream = new FileStream(zipFileTempPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            return zipFileTempPath;
        }
    }
}
