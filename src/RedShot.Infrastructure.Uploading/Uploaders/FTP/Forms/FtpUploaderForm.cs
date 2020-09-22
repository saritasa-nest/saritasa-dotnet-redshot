using System;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Forms
{
    /// <summary>
    /// FTP uploader form.
    /// </summary>
    internal class FtpUploaderForm : Dialog<DialogResult>
    {
        private readonly FtpConfiguration ftpConfiguration;
        private ComboBox accounts;
        private DefaultButton uploadButton;

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount SelectedAccount { get; private set; }

        /// <summary>
        /// Initializes FTP uploader form.
        /// </summary>
        public FtpUploaderForm()
        {
            ftpConfiguration = ConfigurationManager.GetSection<FtpConfiguration>();
            Title = "FTP Upload";
            Size = new Size(350, 280);
            ShowInTaskbar = true;
            InitializeComponents();
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            accounts = new ComboBox()
            {
                DataStore = ftpConfiguration.FtpAccounts,
                Size = new Size(250, 21),
            };

            if (ftpConfiguration.FtpAccounts.Count > 0)
            {
                accounts.SelectedValue = ftpConfiguration.FtpAccounts.First();
            }

            uploadButton = new DefaultButton("Upload", 100, 30);
            uploadButton.Clicked += UploadButton_Click;

            Content = new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical,
                Padding = 20,
                Items =
                {
                    GetAccountLayout(),
                    FormsHelper.GetVoidBox(20),
                    uploadButton
                }
            };
        }

        private StackLayout GetAccountLayout()
        {
            return new StackLayout()
            {
                Padding = 10,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical,
                Spacing = 10,
                Items =
                {
                    new Label()
                    {
                        Text = "Account:",
                        Width = 100,
                    },
                    accounts,
                }
            };
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (accounts.DataStore.Any() && accounts.SelectedValue != null)
            {
                SelectedAccount = (FtpAccount)accounts.SelectedValue;
                Result = DialogResult.Ok;
            }
            else
            {
                Result = DialogResult.Cancel;
            }

            Close();
        }
    }
}
