using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploaders.File
{
    /// <summary>
    /// File uploader.
    /// </summary>
    internal sealed class FileUploader : IUploader
    {
        /// <inheritdoc />
        public async Task<UploadResult> UploadAsync(Uploading.Common.File file, CancellationToken cancellationToken)
        {
            UploadResult result;

            if (file.FileType == FileType.Image)
            {
                result = await UploadImageAsync(file, cancellationToken);
            }
            else
            {
                result = await UploadVideoAsync(file, cancellationToken);
            }

            if (result.ResultType == UploadResultType.Successful)
            {
                NotifyHelper.Notify($"The file has been saved.", "RedShot", NotifyStatus.Success);
            }

            return result;
        }

        private async Task<UploadResult> UploadImageAsync(Uploading.Common.File file, CancellationToken cancellationToken)
        {
            using var dialog = new SaveFileDialog();
            dialog.Title = "RedShot";

            var fileName = FormatManager.GetFormattedName();

            dialog.FileName = fileName;
            dialog.Filters.Add(new FileFilter("PNG Image", ".png"));
            dialog.Filters.Add(new FileFilter("JPEG Image", ".jpeg"));

            if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
            {
                using var image = new Bitmap(file.FilePath);

                switch (dialog.CurrentFilterIndex)
                {
                    case 0:
                        await image.SaveAsync(dialog.FileName, ImageFormat.Jpeg, cancellationToken);
                        break;
                    case 1:
                        await image.SaveAsync(dialog.FileName, ImageFormat.Png, cancellationToken);
                        break;
                    default:
                        var filePath = Path.Combine(dialog.Directory.ToString(), $"{fileName}.png");
                        await image.SaveAsync(filePath, ImageFormat.Png, cancellationToken);
                        break;
                }
                return UploadResult.Successful;
            }
            else
            {
                return UploadResult.Canceled;
            }
        }

        private async Task<UploadResult> UploadVideoAsync(Uploading.Common.File file, CancellationToken cancellationToken)
        {
            using var dialog = new SaveFileDialog();
            dialog.Title = "RedShot";
            var fileInfo = new FileInfo(file.FilePath);

            dialog.FileName = FormatManager.GetFormattedName();
            dialog.Filters.Add(new FileFilter("Video format", $"{fileInfo.Extension}"));

            if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
            {
                await CopyFileAsync(file.FilePath, dialog.FileName, cancellationToken);
                return UploadResult.Successful;
            }
            else
            {
                return UploadResult.Canceled;
            }
        }

        private static async Task CopyFileAsync(string sourceFile, string destinationFile, CancellationToken cancellationToken)
        {
            var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var bufferSize = 4096;

            using var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions);
            using var destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, fileOptions);
            await sourceStream.CopyToAsync(destinationStream, bufferSize, cancellationToken);
        }
    }
}
