using System;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.DataTransfer.Ftp;

namespace RedShot.Infrastructure.Uploaders.Ftp.Settings
{
    internal partial class FtpOptionControl : Panel
    {
        private void InitializeComponents()
        {
            InitializeFileds();

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

        private void InitializeFileds()
        {
            var defaultSize = new Eto.Drawing.Size(200, 21);
            name = new TextBox()
            {
                Size = defaultSize,
            };
            host = new TextBox()
            {
                Size = defaultSize,
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
                Size = defaultSize,
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
                Size = new Eto.Drawing.Size(40, 21),
            };

            ftpProtocol = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpProtocol)).Cast<FtpProtocol>()
                .Select(p => p.ToString()),
                Size = new Eto.Drawing.Size(200, 21),
            };

            ftpsEncryption = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpsEncryption)).Cast<FtpsEncryption>()
                .Select(p => p.ToString()),
                Size = defaultSize,
            };

            accounts = new ComboBox()
            {
                Size = new Eto.Drawing.Size(150, 21),
            };

            addButton = new Button()
            {
                Text = "Add",
                Size = new Eto.Drawing.Size(100, 30),
            };

            addButton.Click += AddButton_Click;

            delButton = new Button()
            {
                Text = "Delete",
                Size = new Eto.Drawing.Size(100, 30),
            };

            delButton.Click += DelButton_Click;

            copyButton = new Button()
            {
                Text = "Copy",
                Size = new Eto.Drawing.Size(100, 30),
            };

            copyButton.Click += CopyButton_Click;

            ftpsCertificateLocationButton = new Button()
            {
                Text = "...",
                Size = new Eto.Drawing.Size(35, 21),
            };

            ftpsCertificateLocationButton.Click += FtpsCertificateLocationButton_Click;

            keyPathButton = new Button()
            {
                Text = "...",
                Size = new Eto.Drawing.Size(35, 21),
            };

            keyPathButton.Click += KeyPathButton_Click;

            ftpProtocol.SelectedValueChanged += FtpProtocol_SelectedValueChanged;

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
                    Items =
                    {
                        BaseAccountBoxes(),
                        ftpsBoxes,
                        sftpBoxes
                    }
                },
                Text = "Account",
                MinimumSize = new Eto.Drawing.Size(600, 400),
                Padding = 20
            };
        }

        private void FtpProtocol_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (Enum.Parse(typeof(FtpProtocol), (string)ftpProtocol.SelectedValue))
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
                Items =
                {
                    FormsHelper.GetBaseStack("Name", name),
                    FormsHelper.GetBaseStack("Ftp Protocol", ftpProtocol),
                    GetAdressBoxes(),
                    FormsHelper.GetBaseStack("Username", username),
                    FormsHelper.GetBaseStack("Password", password),
                    FormsHelper.GetBaseStack("IsActive", isActive),
                    FormsHelper.GetBaseStack("SubFolderPath", subFolderPath),
                }
            };
        }

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
                        Text = "Accounts:",
                    },
                    accounts,
                    addButton,
                    delButton,
                    copyButton
                }
            };
        }

        private StackLayout GetAdressBoxes()
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Padding = 5,
                Spacing = 10,
                Items =
                {
                    new Label()
                    {
                        Text = "Host:",
                    },
                    host,
                    new Label()
                    {
                        Text = "Port:",
                    },
                    port,
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
                        FormsHelper.GetBaseStack("Encryption", ftpsEncryption),
                        new StackLayout()
                        {
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            Orientation = Orientation.Horizontal,
                            Padding = 5,
                            Items =
                            {
                                new StackLayout()
                                {
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Width = 200,
                                    Items =
                                    {
                                        new Label()
                                        {
                                            Text = "Location of the certificate",
                                        }
                                    }
                                },
                                new StackLayout()
                                {
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                    Items =
                                    {
                                        ftpsCertificateLocation,
                                        ftpsCertificateLocationButton
                                    }
                                }
                            }
                        },
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
                            Padding = 5,
                            Orientation = Orientation.Horizontal,
                            Items =
                            {
                                new StackLayout()
                                {
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Width = 200,
                                    Items =
                                    {
                                        new Label()
                                        {
                                            Text = "Location of the key:",
                                        },
                                    }
                                },
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
                                }
                            }
                        },
                        new StackLayout()
                        {
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            Orientation = Orientation.Horizontal,
                            Items =
                            {
                                FormsHelper.GetBaseStack("Pass phrase", passphrase),
                            }
                        }
                    }
                }
            };
        }
    }
}
