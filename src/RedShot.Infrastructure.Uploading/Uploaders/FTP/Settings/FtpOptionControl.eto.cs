using System;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings
{
    internal partial class FtpOptionControl : Panel
    {
        private void InitializeComponents()
        {
            InitializeFields();

            Content = new TableLayout
            {
                Padding = 20,
                Spacing = new Size(0, 10),
                Rows =
                {
                    GetAccountsPanel(),
                    GetAccountFields()
                }
            };
        }

        private Control GetAccountFields()
        {
            if (accountFields == null)
            {
                accountFields = new GroupBox()
                {
                    Enabled = false,
                    Content = new TableLayout()
                    {
                        Padding = new Padding(5, 10),
                        Spacing = new Size(10, 10),
                        Rows =
                        {
                            FormsHelper.CreateFieldStack("Protocol", ftpProtocol),
                            GetAddressBoxes(),
                            CreateAuthenticationFields(),
                            FormsHelper.CreateFieldStack("Directory", directoryPath),
                            GetLinkBoxes(),
                            TableLayout.AutoSized(GetFtpsBoxes(), new Padding(3, 0, 0, 0)),
                            TableLayout.AutoSized(GetSftpBoxes(), new Padding(3, 0, 0, 0)),
                            TableLayout.AutoSized(defaultAccountCheckBox, new Padding(3, 0, 0, 0)),
                            TableLayout.AutoSized(testButton, new Padding(2, 0, 0, 0))
                        }
                    },
                    Text = "Account",
                    Padding = 10,
                };
            }

            return accountFields;
        }

        private void InitializeFields()
        {
            var defaultSize = new Size(170, 21);
            var defaultLongSize = new Size(300, 21);

            addExtensionCheckBox = new CheckBox
            {
                Text = "Append file extension"
            };
            previewLink = new TextBox()
            {
                ReadOnly = true,
                Size = defaultLongSize
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

            directoryPath = new TextBox()
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
                Size = defaultSize
            };

            ftpsEncryption = new ComboBox()
            {
                Size = defaultSize
            };

            defaultAccountCheckBox = new CheckBox
            {
                Text = "Set as default"
            };

            accounts = new ComboBox()
            {
                Size = new Size(250, 21),
                ReadOnly = true
            };

            addButton = new Button()
            {
                Image = Resources.Icons.Add,
                Size = new Size(26, 26),
                ToolTip = "Add account"
            };
            addButton.Click += AddButtonClick;

            delButton = new Button()
            {
                Image = Resources.Icons.Remove,
                Size = new Size(26, 26),
                ToolTip = "Remove account"
            };
            delButton.Click += DelButtonClick;

            copyButton = new Button()
            {
                Image = Resources.Icons.Copy,
                Size = new Size(26, 26),
                ToolTip = "Clone account"
            };
            copyButton.Click += CopyButtonClick;

            ftpsCertificateLocationButton = new DefaultButton("...", 35, 21);
            ftpsCertificateLocationButton.Clicked += FtpsCertificateLocationButtonClick;

            keyPathButton = new DefaultButton("...", 35, 21);
            keyPathButton.Clicked += KeyPathButtonClick;

            ftpProtocol.SelectedValueChanged += FtpProtocolSelectedValueChanged;
        }

        private void FtpProtocolSelectedValueChanged(object sender, EventArgs e)
        {
            if (ftpProtocol.SelectedValue == null)
            {
                return;
            }

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

        private StackLayout CreateAuthenticationFields()
        {
            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Items =
                {
                    FormsHelper.CreateFieldStack("Username", username),
                    FormsHelper.CreateFieldStack("Password", password),
                }
            };
        }

        private StackLayout GetAccountsPanel()
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 5,
                Spacing = 8,
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
                Spacing = 5,
                VerticalContentAlignment = VerticalAlignment.Bottom,
                Items =
                {
                    FormsHelper.CreateFieldStack("Host", host),
                    FormsHelper.CreateFieldStack("Port", port),
                }
            };
        }

        private StackLayout GetLinkBoxes()
        {
            Control pathControl;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pathControl = FormsHelper.CreateFieldStack("Base URL", homePathTextBox);
            }
            else
            {
                pathControl = new StackLayout()
                {
                    Orientation = Orientation.Vertical,
                    Items =
                    {
                        new Label()
                        {
                            Text = "Base URL"
                        },
                        directoryPath
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
                    TableLayout.AutoSized(addExtensionCheckBox, new Padding(3, 0, 0, 0)),
                    FormsHelper.GetVoidBox(5),
                    FormsHelper.CreateFieldStack("URL preview", previewLink)
                }
            };
        }

        private Control GetFtpsBoxes()
        {
            if (ftpsBoxes == null)
            {
                ftpsBoxes = new GroupBox()
                {
                    Visible = false,
                    Padding = 5,
                    Text = "FTPS",
                    Content = new StackLayout
                    {
                        Spacing = 5,
                        Orientation = Orientation.Vertical,
                        Items =
                        {
                            FormsHelper.CreateFieldStack("Encryption", ftpsEncryption),
                            FormsHelper.CreateFieldStack("Location of the certificate", new StackLayout()
                            {
                                Spacing = 5,
                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                Orientation = Orientation.Horizontal,
                                Items =
                                {
                                    ftpsCertificateLocation,
                                    ftpsCertificateLocationButton
                                }
                            })
                        }
                    }
                };
            }

            return ftpsBoxes;
        }

        private Control GetSftpBoxes()
        {
            if (sftpBoxes == null)
            {
                sftpBoxes = new GroupBox()
                {
                    Visible = false,
                    Padding = 5,
                    Text = "SFTP",
                    Content = new StackLayout
                    {
                        Padding = 5,
                        Orientation = Orientation.Vertical,
                        Items =
                        {
                            new StackLayout()
                            {
                                Orientation = Orientation.Horizontal,
                                Items =
                                {
                                    FormsHelper.CreateFieldStack("RSA Key",
                                    new StackLayout()
                                    {
                                        Spacing = 5,
                                        HorizontalContentAlignment = HorizontalAlignment.Left,
                                        Orientation = Orientation.Horizontal,
                                        Items =
                                        {
                                            keypath,
                                            keyPathButton
                                        }
                                    }),
                                }
                            },
                            FormsHelper.CreateFieldStack("Pass phrase", passphrase)
                        }
                    }
                };
            }

            return sftpBoxes;
        }
    }
}