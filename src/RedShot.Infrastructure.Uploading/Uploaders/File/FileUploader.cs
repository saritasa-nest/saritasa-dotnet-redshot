using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Uploaders.File
{
    /// <summary>
    /// File uploader.
    /// </summary>
    internal sealed class FileUploader : IUploader
    {
        /// <inheritdoc/>
        public event EventHandler<UploadingFinishedEventArgs> UploadingFinished;

        /// <inheritdoc />
        public async Task<IUploadingResponse> UploadAsync(IFile file, CancellationToken cancellationToken)
        {
            if (file.FileType == FileType.Image)
            {
                return await UploadImageAsync(file, cancellationToken);
            }
            else
            {
                return await UploadVideoAsync(file, cancellationToken);
            }
        }

        private async Task<IUploadingResponse> UploadImageAsync(IFile file, CancellationToken cancellationToken)
        {
            using var dialog = new SaveFileDialog();
            dialog.Title = "RedShot";
            dialog.FileName = $"{file.FileName}";
            dialog.Filters.Add(new FileFilter("Png format", ".png"));
            dialog.Filters.Add(new FileFilter("Jpeg format", ".jpeg"));

            if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
            {
                var image = file.GetFilePreview();

                switch (dialog.CurrentFilterIndex)
                {
                    case 0:
                        await image.SaveAsync(dialog.FileName, ImageFormat.Jpeg, cancellationToken);
                        break;
                    case 1:
                        await image.SaveAsync(dialog.FileName, ImageFormat.Png, cancellationToken);
                        break;
                    default:
                        var fileName = Path.Combine(dialog.Directory.ToString(), $"{file.FileName}.png");
                        await image.SaveAsync(fileName, ImageFormat.Png, cancellationToken);
                        break;
                }

                UploadingFinished?.Invoke(this, UploadingFinishedEventArgs.CreateNew(file));
            }

            return new BaseUploadingResponse(true);
        }

        private async Task<IUploadingResponse> UploadVideoAsync(IFile file, CancellationToken cancellationToken)
        {
            using var dialog = new SaveFileDialog();
            dialog.Title = "RedShot";
            var extension = Path.GetExtension(file.FilePath);
            dialog.FileName = $"{file.FileName}{extension}";
            dialog.Filters.Add(new FileFilter("Video format", $"{extension}"));

            if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
            {
                await CopyFileAsync(file.FilePath, dialog.FileName, cancellationToken);

                UploadingFinished?.Invoke(this, UploadingFinishedEventArgs.CreateNew(file));
                return new BaseUploadingResponse(true);
            }

            return new BaseUploadingResponse(false);
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
