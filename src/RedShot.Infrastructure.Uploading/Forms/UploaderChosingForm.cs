using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using System.Collections.Generic;

namespace RedShot.Infrastructure.Uploading.Forms
{
    internal class UploaderChosingForm : Form
    {
        private readonly IFile file;
        private readonly IEnumerable<IUploadingService> uploadingServices;

        public UploaderChosingForm(IFile file, IEnumerable<IUploadingService> uploadingServices)
        {
            Title = $"{file.FileType} uploading";
            this.file = file;
            this.uploadingServices = uploadingServices;
            InitializeComponents();
            this.Shown += UploaderChosingForm_Shown;
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
            foreach (var service in uploadingServices)
            {
                stack.Items.Add(GetServiceButton(service));
                stack.Items.Add(FormsHelper.VoidBox(10));
            }

            Content = stack;
        }

        private Control GetServiceButton(IUploadingService service)
        {
            var button = new ImageButton(new Size(100, 100), service.ServiceImage);
            button.Clicked += (o, e) =>
            {
                var uploader = service.GetUploader();
                UploadManager.Upload(uploader, file);
            };

            button.Enabled = service.CheckOnSupporting(file.FileType);

            return button;
        }
    }
}
