using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Uploading.Forms
{
    /// <summary>
    /// Uploader choosing form.
    /// </summary>
    internal class UploaderChosingForm : Form
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IFile file;
        private readonly IEnumerable<IUploadingService> uploadingServices;

        /// <summary>
        /// Initializes uploader choosing form.
        /// </summary>
        public UploaderChosingForm(IFile file, IEnumerable<IUploadingService> uploadingServices)
        {
            Title = $"{file.FileType} uploading";
            this.file = file;
            this.uploadingServices = uploadingServices;
            InitializeComponents();
            this.Shown += UploaderChosingForm_Shown;
            Resizable = false;
        }

        private void UploaderChosingForm_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            var stack = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                Padding = 10,
            };

            stack.Items.Add(FormsHelper.VoidBox(10));
            stack.Items.Add(GetPreviewButton());
            stack.Items.Add(FormsHelper.VoidBox(10));
            foreach (var service in uploadingServices)
            {
                stack.Items.Add(GetServiceButton(service));
                stack.Items.Add(FormsHelper.VoidBox(10));
            }

            Content = stack;
        }

        private Control GetPreviewButton()
        {
            var previewIcon = new Bitmap(Resources.Properties.Resources.Open);
            var button = new ImageButton(new Size(100, 100), previewIcon)
            {
                ToolTip = "Opens the file"
            };

            button.Clicked += (o, e) =>
            {
                OpenFile(file);
            };

            return button;
        }

        private Control GetServiceButton(IUploadingService service)
        {
            var button = new ImageButton(new Size(100, 100), service.ServiceImage)
            {
                ToolTip = service.About
            };

            button.Clicked += (o, e) =>
            {
                var uploader = service.GetUploader();
                UploadingManager.Upload(uploader, file);
            };

            button.Enabled = service.CheckOnSupporting(file.FileType);

            return button;
        }

        private void OpenFile(IFile file)
        {
            if (!string.IsNullOrEmpty(file.FilePath))
            {
                Task.Run(() =>
                {
                    try
                    {
                        Process.Start(
                            new ProcessStartInfo
                            {
                                FileName = file.FilePath,
                                UseShellExecute = true
                            });
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, $"An error occurred in opening {file.FilePath}");
                    }
                });
            }
        }
    }
}
