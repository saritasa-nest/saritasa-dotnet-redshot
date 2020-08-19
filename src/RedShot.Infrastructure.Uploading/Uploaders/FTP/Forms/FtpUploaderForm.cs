using System;
using Eto.Forms;
using Eto.Drawing;
using System.Collections.Generic;
using System.Linq;
using RedShot.Infrastructure.DataTransfer.Ftp;

namespace RedShot.Infrastructure.Uploaders.Ftp.Forms
{
    public partial class FtpUploaderForm : Dialog
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private List<FtpAccount> ftpAccounts;

        private ComboBox accounts;

        private Button uploadButton;
        private Button ftpSettingsButton;
        private TextBox imageNameBox;

        public FtpAccount SelectedAccount { get; private set; }

        public FtpUploaderForm(FtpConfiguration configuration)
        {
            ftpAccounts = configuration.FtpAccounts;

            Title = "FTP Upload";
            Size = new Size(350, 280);

            ShowInTaskbar = true;

            InitializeComponents();

            ftpSettingsButton.Click += FtpSettingsButton_Click;
            uploadButton.Click += UploadButton_Click;

            Location = FormsHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            accounts = new ComboBox()
            {
                DataStore = ftpAccounts,
                Size = new Eto.Drawing.Size(250, 21),
            };

            imageNameBox = new TextBox()
            {
                Size = new Eto.Drawing.Size(250, 21),
            };

            uploadButton = new Button()
            {
                Text = "Upload",
            };

            ftpSettingsButton = new Button()
            {
                Text = "Settings",
            };

            Content = new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical,
                Padding = 20,
                Items =
                {
                    GetAccountLayout(),
                    GetImageNameLayout(),
                    FormsHelper.VoidBox(20),
                    GetButtonsLayout()
                }
            };
        }

        private StackLayout GetButtonsLayout()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    uploadButton,
                    FormsHelper.VoidBox(20),
                    ftpSettingsButton
                }
            };
        }

        private StackLayout GetImageNameLayout()
        {
            return new StackLayout()
            {
                Padding = 10,
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Items =
                {
                    new Label()
                    {
                        Text = "Image name",
                        Width = 100,
                    },
                    FormsHelper.VoidBox(10),
                    imageNameBox
                }
            };
        }

        private StackLayout GetAccountLayout()
        {
            return new StackLayout()
            {
                Padding = 10,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical,
                Items =
                {
                    new Label()
                    {
                        Text = "Select account",
                        Width = 100,
                    },
                    FormsHelper.VoidBox(10),
                    accounts,
                }
            };
        }

        private void FtpSettingsButton_Click(object sender, EventArgs e)
        {
            var form = new FtpConfig();
            form.ShowModal();
            accounts.SelectedIndex = -1;
            accounts.DataStore = ftpAccounts;
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (accounts.DataStore.Count() > 0 && accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;
                var service = new FtpUploadingService(acc);

                string imageName;

                if (string.IsNullOrEmpty(imageNameBox.Text))
                {
                    imageName = $"{Guid.NewGuid()}.png";
                }
                else
                {
                    imageName = $"{imageNameBox.Text}.png";
                }

                try
                {
                    var uploader = service.CreateUploader();

                    var response = uploader.UploadImage(image, imageName, ImageFormat.Png);

                    if (response.IsSuccess)
                    {
                        Logger.Trace("Image uploaded to FTP server", response);
                        MessageBox.Show("Image uploaded", "Success", MessageBoxButtons.OK, MessageBoxType.Information);
                    }
                    else
                    {
                        Logger.Trace("Image uploaded to FTP server failed", response);
                        MessageBox.Show("Image uploading failed", MessageBoxButtons.OK, MessageBoxType.Information);
                    }

                    if (uploader is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ftp uploading error");
                    MessageBox.Show(ex.Message, "Ftp upload error!", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            }
        }
    }
}
