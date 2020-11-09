using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;

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
        public IUploadingResponse Upload(IFile file)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "RedShot";

                if (file.FileType == FileType.Image)
                {
                    dialog.FileName = $"{file.FileName}";
                    dialog.Filters.Add(new FileFilter("Png format", ".png"));
                    dialog.Filters.Add(new FileFilter("Jpeg format", ".jpeg"));

                    if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
                    {
                        var image = file.GetFilePreview();

                        switch (dialog.CurrentFilterIndex)
                        {
                            case 0:
                                image.Save(dialog.FileName, ImageFormat.Jpeg);
                                break;
                            case 1:
                                image.Save(dialog.FileName, ImageFormat.Png);
                                break;
                            default:
                                image.Save(Path.Combine(dialog.Directory.ToString(), $"{file.FileName}.png"), ImageFormat.Png);
                                break;
                        }

                        UploadingFinished?.Invoke(this, UploadingFinishedEventArgs.CreateNew(file));
                        return new BaseUploadingResponse(true);
                    }
                }
                else
                {
                    var extension = Path.GetExtension(file.FilePath);

                    dialog.FileName = $"{file.FileName}{extension}";
                    dialog.Filters.Add(new FileFilter("Video format", $"{extension}"));

                    if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
                    {
                        System.IO.File.Copy(file.FilePath, dialog.FileName);

                        UploadingFinished?.Invoke(this, UploadingFinishedEventArgs.CreateNew(file));
                        return new BaseUploadingResponse(true);
                    }
                }
            }

            return new BaseUploadingResponse(false);
        }
    }
}
