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
        public async Task UploadAsync(Uploading.Common.File file, CancellationToken cancellationToken)
        {
            if (file.FileType == FileType.Image)
            {
                await UploadImageAsync(file, cancellationToken);
            }
            else
            {
                await UploadVideoAsync(file, cancellationToken);
            }

            NotifyHelper.Notify($"The file has been saved.", "RedShot", NotifyStatus.Success);
        }

        private async Task UploadImageAsync(Uploading.Common.File file, CancellationToken cancellationToken)
        {
            using var dialog = new SaveFileDialog();
            dialog.Title = "RedShot";

            var fileName = FormatManager.GetFormattedName();

            dialog.FileName = fileName;
            dialog.Filters.Add(new FileFilter("PNG Image", ".png"));
            dialog.Filters.Add(new FileFilter("JPEG Image", ".jpeg"));

            if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
            {
                using var fileStream = file.GetStream();
                using var image = new Bitmap(file.GetStream());

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
            }
        }

        private async Task UploadVideoAsync(Uploading.Common.File file, CancellationToken cancellationToken)
        {
            using var dialog = new SaveFileDialog();
            dialog.Title = "RedShot";
            var fileInfo = new FileInfo(file.FilePath);

            dialog.FileName = FormatManager.GetFormattedName();
            dialog.Filters.Add(new FileFilter("Video format", $"{fileInfo.Extension}"));

            if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
            {
                await CopyFileAsync(file.FilePath, dialog.FileName, cancellationToken);
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
