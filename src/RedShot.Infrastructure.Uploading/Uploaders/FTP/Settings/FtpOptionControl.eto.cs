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
                Size = defaultSize
            };

            ftpsEncryption = new ComboBox()
            {
                Size = defaultSize
            };

            accounts = new ComboBox()
            {
                Size = new Size(250, 21)
            };

            const string setDefault = "Set as default";
            defaultProtocolCheckBox = new CheckBox
            {
                ToolTip = setDefault
            };

            defaultHostCheckBox = new CheckBox
            {
                ToolTip = setDefault
            };

            defaultUserCheckBox = new CheckBox
            {
                ToolTip = setDefault
            };

            defaultDirectoryCheckBox = new CheckBox
            {
                ToolTip = setDefault
            };

            defaultBaseUrlCheckBox = new CheckBox
            {
                ToolTip = setDefault
            };

            defaultFtpsConfigurationCheckBox = new CheckBox
            {
                Text = "Set as default"
            };

            defaultSftpConfigurationCheckBox = new CheckBox
            {
                Text = "Set as default"
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
                Size = new Size(435, 650),
                Text = "Account",
                Padding = 10
            };
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

        private StackLayout BaseAccountBoxes()
        {
            var protocolFields = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Items =
                {
                    ftpProtocol,
                    defaultProtocolCheckBox
                }
            };

            var directoryFields = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Items =
                {
                    subFolderPath,
                    defaultDirectoryCheckBox
                }
            };

            return new StackLayout
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 5,
                Items =
                {
                    FormsHelper.CreateFieldStack("Protocol", protocolFields),
                    GetAddressBoxes(),
                    CreateAuthenticationFields(),
                    FormsHelper.CreateFieldStack("Directory", directoryFields),
                    GetLinkBoxes(),
                }
            };
        }

        private StackLayout CreateAuthenticationFields()
        {
            // This is only necessary for correct default user data checkbox alignment.
            // And I'm not sure this is a good solution.
            var passwordStack = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    password,
                    defaultUserCheckBox
                }
            };

            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Items =
                {
                    FormsHelper.CreateFieldStack("Username", username),
                    FormsHelper.CreateFieldStack("Password", passwordStack),
                    defaultUserCheckBox
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

        private StackLayout GetAddressBoxes()
        {
            // This is only necessary for correct default host checkbox alignment.
            // And I'm not sure this is a good solution.
            var portStack = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    port,
                    defaultHostCheckBox
                }
            };

            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                VerticalContentAlignment = VerticalAlignment.Bottom,
                Items =
                {
                    FormsHelper.CreateFieldStack("Host", host),
                    FormsHelper.CreateFieldStack("Port", portStack),
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
                    homePathTextBox,
                    defaultBaseUrlCheckBox
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
                    Spacing = 5,
                    Padding = 5,
                    Orientation = Orientation.Vertical,
                    Items =
                    {
                        FormsHelper.CreateFieldStack("Encryption", ftpsEncryption, 0),
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
                        }, 0),
                        defaultFtpsConfigurationCheckBox
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
                    Padding = 5,
                    Width = 550,
                    Spacing = 5,
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
                                    Spacing = 5,
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Orientation = Orientation.Horizontal,
                                    Items =
                                    {
                                        keypath,
                                        keyPathButton
                                    }
                                }, 0),
                            }
                        },
                        FormsHelper.CreateFieldStack("Pass phrase", passphrase, 0),
                        defaultSftpConfigurationCheckBox
                    }
                }
            };
        }
    }
}