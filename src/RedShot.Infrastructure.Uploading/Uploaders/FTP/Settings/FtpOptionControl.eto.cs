using System;
using System.Linq;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings
{
    internal partial class FtpOptionControl : Panel
    {
        private void InitializeComponents()
        {
            InitializeFields();

            Content = new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Vertical,
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    GetAccountsPanel(),
                    accountFields,
                }
            };
        }

        private void InitializeFields()
        {
            var defaultSize = new Size(170, 21);
            var defaultLongSize = new Size(300, 21);

            addExtensionCheckBox = new CheckBox();
            previewLinkLabel = new Label();
            browserTypeComboBox = new ComboBox
            {
                Size = new Size(75, 21)
            };
            homePathTextBox = new TextBox()
            {
                Size = defaultLongSize
            };

            testButton = new DefaultButton("Test connection", 100, 25);
            testButton.Clicked += TestButtonClicked;

            host = new TextBox()
            {
                Size = defaultSize
            };

            username = new TextBox()
            {
                Size = defaultSize,
            };

            password = new PasswordBox()
            {
                Size = defaultSize,
            };

            subFolderPath = new TextBox()
            {
                Size = defaultLongSize,
            };

            ftpsCertificateLocation = new TextBox()
            {
                Size = defaultSize,
            };

            keypath = new TextBox()
            {
                Size = defaultSize,
            };

            passphrase = new PasswordBox()
            {
                Size = defaultSize,
            };

            port = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 65535,
                Increment = 1,
                Value = 21,
                Size = new Size(60, 21),
            };

            ftpProtocol = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpProtocol)).Cast<FtpProtocol>()
                    .Select(p => p.ToString()),
                Size = defaultSize,
            };

            ftpsEncryption = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpsEncryption)).Cast<FtpsEncryption>()
                    .Select(p => p.ToString()),
                Size = defaultSize
            };

            accounts = new ComboBox()
            {
                Size = new Size(250, 21)
            };

            addButton = new Button()
            {
                Image = Resources.Icons.Add,
                Size = new Size(26, 26)
            };
            addButton.Click += AddButtonClick;

            delButton = new Button()
            {
                Image = Resources.Icons.Remove,
                Size = new Size(26, 26)
            };
            delButton.Click += DelButtonClick;

            copyButton = new Button()
            {
                Image = Resources.Icons.Copy,
                Size = new Size(26, 26)
            };
            copyButton.Click += CopyButtonClick;

            ftpsCertificateLocationButton = new DefaultButton("...", 35, 21);
            ftpsCertificateLocationButton.Clicked += FtpsCertificateLocationButtonClick;

            keyPathButton = new DefaultButton("...", 35, 21);
            keyPathButton.Clicked += KeyPathButtonClick;

            ftpProtocol.SelectedValueChanged += FtpProtocolSelectedValueChanged;

            ftpsBoxes = GetFtpsBoxes();
            ftpsBoxes.Visible = false;

            sftpBoxes = GetSftpBoxes();
            sftpBoxes.Visible = false;

            ftpsEncryption.SelectedIndex = 0;
            ftpProtocol.SelectedIndex = 0;

            accountFields = GetAccountFieldsControl();
            accountFields.Enabled = false;
        }

        private Control GetAccountFieldsControl()
        {
            return new GroupBox()
            {
                Content = new StackLayout()
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 10,
                    Items =
                    {
                        BaseAccountBoxes(),
                        ftpsBoxes,
                        sftpBoxes,
                        testButton
                    }
                },
                Size = new Size(435, 550),
                Text = "Account",
                Padding = 10
            };
        }

        private void FtpProtocolSelectedValueChanged(object sender, EventArgs e)
        {
            switch (Enum.Parse(typeof(FtpProtocol), (string) ftpProtocol.SelectedValue))
            {
                case FtpProtocol.FTP:
                    ftpsBoxes.Visible = false;
                    sftpBoxes.Visible = false;
                    break;

                case FtpProtocol.FTPS:
                    sftpBoxes.Visible = false;
                    ftpsBoxes.Visible = true;
                    break;

                case FtpProtocol.SFTP:
                    ftpsBoxes.Visible = false;
                    sftpBoxes.Visible = true;
                    break;
            }
        }

        private StackLayout BaseAccountBoxes()
        {
            return new StackLayout
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 5,
                Items =
                {
                    FormsHelper.CreateFieldStack("Protocol", ftpProtocol),
                    GetAddressBoxes(),
                    CreateAuthenticationFields(),
                    FormsHelper.CreateFieldStack("Directory", subFolderPath),
                    GetLinkBoxes(),
                }
            };
        }

        private StackLayout CreateAuthenticationFields() =>
            new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 20,
                Items =
                {
                    FormsHelper.CreateFieldStack("Username", username),
                    FormsHelper.CreateFieldStack("Password", password)
                }
            };

        private StackLayout GetAccountsPanel()
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 10,
                Spacing = 10,
                Orientation = Orientation.Horizontal,
                Items =
                {
                    new Label()
                    {
                        Text = "Accounts",
                    },
                    accounts,
                    addButton,
                    delButton,
                    copyButton
                }
            };
        }

        private StackLayout GetAddressBoxes()
        {
            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 20,
                Items =
                {
                    FormsHelper.CreateFieldStack("Host", host),
                    FormsHelper.CreateFieldStack("Port", port)
                }
            };
        }

        private StackLayout GetLinkBoxes()
        {
            var pathBoxes = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    homePathTextBox
                }
            };

            Control pathControl;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pathControl = FormsHelper.CreateFieldStack("Base URL", pathBoxes);
            }
            else
            {
                pathControl = new StackLayout()
                {
                    Orientation = Orientation.Vertical,
                    Padding = 5,
                    Items =
                    {
                        new Label()
                        {
                            Text = "Base URL"
                        },
                        pathBoxes
                    }
                };
            }

            return new StackLayout
            {
                Orientation = Orientation.Vertical,
                Spacing = 5,
                Items =
                {
                    pathControl,
                    FormsHelper.GetBaseStack("Append file extension", addExtensionCheckBox, nameWidth: 150),
                    FormsHelper.GetBaseStack("URL preview", previewLinkLabel, controlWidth: 300)
                }
            };
        }

        private GroupBox GetFtpsBoxes()
        {
            return new GroupBox()
            {
                Text = "FTPS",
                Width = 550,
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Items =
                    {
                        FormsHelper.CreateFieldStack("Encryption", ftpsEncryption),
                        FormsHelper.CreateFieldStack("Location of the certificate", new StackLayout()
                        {
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            Orientation = Orientation.Horizontal,
                            Spacing = 5,
                            Items =
                            {
                                ftpsCertificateLocation,
                                ftpsCertificateLocationButton
                            }
                        }),
                    }
                }
            };
        }

        private GroupBox GetSftpBoxes()
        {
            return new GroupBox()
            {
                Text = "SFTP",
                Content = new StackLayout
                {
                    Width = 550,
                    Orientation = Orientation.Vertical,
                    Items =
                    {
                        new StackLayout()
                        {
                            Spacing = 5,
                            Orientation = Orientation.Horizontal,
                            Items =
                            {
                                FormsHelper.CreateFieldStack("RSA Key",
                                new StackLayout()
                                {
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                    Items =
                                    {
                                        keypath,
                                        keyPathButton
                                    }
                                })
                            }
                        },
                        FormsHelper.CreateFieldStack("Pass phrase", passphrase)
                    }
                }
            };
        }
    }
}