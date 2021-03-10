using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Forms
{
    /// <summary>
    /// Uploader choosing form.
    /// </summary>
    public class UploadingForm : Form
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly File file;
        private readonly IEnumerable<IUploadingService> uploadingServices;

        /// <summary>
        /// Initializes uploader choosing form.
        /// </summary>
        public UploadingForm(File file, ICollection<IUploadingService> uploadingServices)
        {
            Title = $"{file.FileType} uploading";
            this.file = file;
            this.uploadingServices = uploadingServices;
            InitializeComponents();
            this.Shown += UploaderChoosingForm_Shown;
            Resizable = false;
        }

        private void UploaderChoosingForm_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            var stack = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                Padding = 10,
                Spacing = 10
            };

            stack.Items.Add(GetPreviewButton());
            foreach (var service in uploadingServices)
            {
                stack.Items.Add(GetServiceButton(service));
            }

            Content = stack;
        }

        private Control GetPreviewButton()
        {
            var previewIcon = Icons.Open;
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

            button.Clicked += async (o, e) => await UploadAsync(service);
            button.Enabled = service.CheckOnSupporting(file.FileType);
            return button;
        }

        private async Task UploadAsync(IUploadingService uploadingService)
        {
            Enabled = false;

            var uploader = uploadingService.GetUploader();
            await UploadingProvider.SafeUploadAsync(uploader, file);
            Close();
        }

        private void OpenFile(File file)
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
