using System;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.DataTransfer.Ftp;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Uploaders.Ftp.Forms
{
    public partial class FtpUploaderForm : Dialog<DialogResult>
    {
        private FtpConfiguration ftpConfiguration;
        private ComboBox accounts;
        private Button uploadButton;
        private TextBox fileNameBox;

        public string FileName { get; private set; }

        public FtpAccount SelectedAccount { get; private set; }

        public FtpUploaderForm()
        {
            ftpConfiguration = ConfigurationManager.GetSection<FtpConfiguration>();
            Title = "FTP Upload";
            Size = new Size(350, 280);

            ShowInTaskbar = true;

            InitializeComponents();
            uploadButton.Click += UploadButton_Click;

            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            accounts = new ComboBox()
            {
                DataStore = ftpConfiguration.FtpAccounts,
                Size = new Eto.Drawing.Size(250, 21),
            };

            fileNameBox = new TextBox()
            {
                Size = new Eto.Drawing.Size(250, 21),
            };

            uploadButton = new Button()
            {
                Text = "Upload",
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
                    uploadButton
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
                        Text = "File name",
                        Width = 100,
                    },
                    FormsHelper.VoidBox(10),
                    fileNameBox
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

        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (accounts.DataStore.Count() > 0 && accounts.SelectedValue != null)
            {
                SelectedAccount = (FtpAccount)accounts.SelectedValue;
                FileName = fileNameBox.Text;
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
