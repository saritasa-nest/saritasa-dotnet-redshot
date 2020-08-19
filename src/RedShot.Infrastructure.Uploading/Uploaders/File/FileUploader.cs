﻿using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Abstractions;
using RedShot.Abstractions.Uploading;
using RedShot.Upload.Basics;

namespace RedShot.Infrastructure.Uploaders.File
{
    public class FileUploader : IUploader
    {
        public IUploadingResponse Upload(IFile file)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "RedShot";

                if (file.FileType == FileType.Image)
                {
                    dialog.FileName = $"{DateTime.Now.ToFileTime()}.bmp";
                    dialog.Filters.Add(new FileFilter("Bmp format", ".bmp"));
                    dialog.Filters.Add(new FileFilter("Png format", ".png"));

                    if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
                    {
                        var image = file.GetFilePreview();

                        switch (dialog.CurrentFilterIndex)
                        {
                            case 0:
                                image.Save(dialog.FileName, ImageFormat.Bitmap);
                                break;
                            case 1:
                                image.Save(dialog.FileName, ImageFormat.Png);
                                break;
                            default:
                                image.Save(Path.Combine(dialog.Directory.ToString(), $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);
                                break;
                        }
                    }
                }
                else
                {
                    var extension = Path.GetExtension(file.GetFilePath());

                    dialog.FileName = $"{file.FileName}.{extension}";
                    dialog.Filters.Add(new FileFilter("Video format", $".{extension}"));

                    if (dialog.ShowDialog(new Form()) == DialogResult.Ok)
                    {
                        System.IO.File.Copy(file.GetFilePath(), dialog.FileName);
                    }
                }
            }

            return new BaseUploadingResponse(true);
        }
    }
}
