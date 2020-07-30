using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Helpers.FtpModels;
using System.Collections.Generic;
using RedShot.Configuration;
using System.Linq;
using RedShot.Upload.Uploaders.FTP;
using RedShot.Upload.Basics;
using RedShot.Helpers;

namespace RedShot.Upload.Forms.Ftp
{
    public partial class FtpUploaderForm : Dialog
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private List<FtpAccount> ftpAccounts => ConfigurationManager.YamlConfig.FtpAccounts;
        private ComboBox accounts;
        private Button uploadButton;
        private Button ftpSettingsButton;
        private TextBox imageNameBox;
        private Bitmap image;
        public FtpUploaderForm(Bitmap image)
        {
            Title = "FTP Upload";
            Size = new Size(350, 300);

            ShowInTaskbar = true;

            SetLocation();
            InitializeComponents();

            ftpSettingsButton.Click += FtpSettingsButton_Click;
            uploadButton.Click += UploadButton_Click;

            this.image = image;
        }

        private void InitializeComponents()
        {
            accounts = new ComboBox()
            {
                DataStore = ftpAccounts,
                Size = new Eto.Drawing.Size(150, 21)
            };

            imageNameBox = new TextBox()
            {
                Size = new Eto.Drawing.Size(150, 21)
            };

            uploadButton = new Button()
            {
                Text = "Upload",
            };

            ftpSettingsButton = new Button()
            {
                Text = "Ftp Settings"
            };

            Content = new StackLayout
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical,
                Padding = 20,
                Items =
                {
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        Items =
                        {
                            new Label()
                            {
                                Text = "Account:",
                                Width = 100
                            },
                            accounts
                        }
                    },
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        Items =
                        {
                            new Label()
                            {
                                Text = "Image name:",
                                Width = 100
                            },
                            imageNameBox
                        }
                    },
                    uploadButton,
                    ftpSettingsButton
                }
            };
        }

        private void SetLocation()
        {
            var center = ScreenHelper.GetCentralCoordsOfScreen();

            var location = new Point(center.X - Size.Width / 2, center.Y - Size.Height / 2);

            if (location.X >= 0 && location.Y >= 0)
            {
                Location = location;
            }
            else
            {
                Location = center;
            }
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
                var service = new FtpUploaderService(acc);

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
                    using (var uploader = (BaseUploader)service.CreateUploader())
                    {
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
