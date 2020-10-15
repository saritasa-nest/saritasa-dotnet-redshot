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
            InitializeFileds();

            Content = new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Vertical,
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    GetPrimaryAccountSelectionFields(),
                    GetAccountsPanel(),
                    accountFields,
                }
            };
        }

        private Control GetPrimaryAccountSelectionFields()
        {
            return FormsHelper.GetBaseStack("Primary account:", primaryAccountSelectionComboBox, nameWidth: 100, padding: 10);
        }

        private void InitializeFileds()
        {
            var defaultSize = new Size(200, 21);

            isActive = new CheckBox();
            addExtensionCheckBox = new CheckBox();
            previewLinkLabel = new Label();
            browserTypeComboBox = new ComboBox();
            homePathTextBox = new TextBox()
            {
                Size = defaultSize
            };

            testButton = new DefaultButton("Test connection", 100, 25);
            testButton.Clicked += TestButtonClicked;

            name = new TextBox()
            {
                Size = defaultSize,
            };
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
                Size = new Size(60, 21),
            };

            ftpProtocol = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpProtocol)).Cast<FtpProtocol>()
                .Select(p => p.ToString()),
                Size = new Size(200, 21),
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

            primaryAccountSelectionComboBox = new ComboBox()
            {
                Size = new Size(250, 21)
            };

            addButton = new Button()
            {
                Text = "Add",
                Size = new Size(100, 30)
            };
            addButton.Click += AddButtonClick;

            delButton = new Button()
            {
                Text = "Delete",
                Size = new Size(100, 30),
            };
            delButton.Click += DelButtonClick;

            copyButton = new Button()
            {
                Text = "Copy",
                Size = new Size(100, 30),
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
                Text = "Account",
                MinimumSize = new Size(700, 400),
                Padding = 20
            };
        }

        private void FtpProtocolSelectedValueChanged(object sender, EventArgs e)
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
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 5,
                Items =
                {
                    FormsHelper.GetBaseStack("Name", name),
                    FormsHelper.GetBaseStack("FTP Protocol", ftpProtocol),
                    GetAdressBoxes(),
                    FormsHelper.GetBaseStack("Username", username),
                    FormsHelper.GetBaseStack("Password", password),
                    FormsHelper.GetBaseStack("IsActive", isActive),
                    FormsHelper.GetBaseStack("SubFolderPath", subFolderPath),
                    GetLinkBoxes(),
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
                        Text = "Accounts",
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
                    port
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
                    browserTypeComboBox,
                    homePathTextBox
                }
            };

            Control pathControl;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pathControl = FormsHelper.GetBaseStack("URL path:", pathBoxes, controlWidth: 400);
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
                            Text = "URL path:"
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
                    FormsHelper.GetBaseStack("Add extension to the URL path", addExtensionCheckBox),
                    FormsHelper.GetBaseStack("URL preview:", previewLinkLabel, controlWidth: 300)
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
