using Eto.Forms;
using RedShot.Helpers.FtpModels;
using System;
using System.Linq;

namespace RedShot.Upload.Forms.Ftp
{
    internal partial class FtpConfig : Dialog
    {
        private ComboBox accounts;
        private Button addButton;
        private Button delButton;
        private Button copyButton;
        private Button saveButton;
        private TextBox name;

        private ComboBox ftpProtocol;
        private TextBox host;
        private NumericStepper port;
        private TextBox username;
        private MaskedTextBox password;
        private CheckBox isActive = new CheckBox();
        private TextBox subFolderPath;

        //private ComboBox browserProtocol;
        //private TextBox httpHomePath = new TextBox();
        //private CheckBox httpHomePathAutoAddSubFolderPath = new CheckBox();
        //private CheckBox httpHomePathNoExtension = new CheckBox();

        private ComboBox ftpsEncryption;
        private TextBox ftpsCertificateLocation;
        private TextBox keypath;
        private MaskedTextBox passphrase;

        private Button ftpsCertificateLocationButton;
        private Button keyPathButton;

        private Control ftpsBoxes;
        private Control sftpBoxes;

        private void InitializeComponents()
        {
            name = new TextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };
            host = new TextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };

            username = new TextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };

            password = new MaskedTextBox()
            {
                Size = new Eto.Drawing.Size(200, 21),
                ShowPromptMode = ShowPromptMode.Always
            };

            subFolderPath = new TextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };

            ftpsCertificateLocation = new TextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };

            keypath = new TextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };

            passphrase = new MaskedTextBox()
            {
                Size = new Eto.Drawing.Size(200, 21)
            };

            port = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 65535,
                Increment = 1,
                Value = 21, 
                Size = new Eto.Drawing.Size(40, 21)
            };

            ftpProtocol = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpProtocol)).Cast<FtpProtocol>()
                .Select(p => p.ToString()),
                Size = new Eto.Drawing.Size(200, 21)
            };

            //browserProtocol = new ComboBox()
            //{
            //    DataStore = Enum.GetValues(typeof(BrowserProtocol)).Cast<BrowserProtocol>()
            //    .Select(p => p.ToString()),
            //    Size = new Eto.Drawing.Size(200, 21)
            //};

            ftpsEncryption = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpsEncryption)).Cast<FtpsEncryption>()
                .Select(p => p.ToString()),
                Size = new Eto.Drawing.Size(200, 21)
            };

            accounts = new ComboBox()
            {
                DataStore = ftpAccounts,
                Size = new Eto.Drawing.Size(150, 21)
            };

            addButton = new Button()
            {
                Text = "Add",
                Size = new Eto.Drawing.Size(100, 30)
            };

            addButton.Click += AddButton_Click;

            delButton = new Button()
            {
                Text = "Delete",
                Size = new Eto.Drawing.Size(100, 30)
            };

            delButton.Click += DelButton_Click;

            copyButton = new Button()
            {
                Text = "Copy",
                Size = new Eto.Drawing.Size(100, 30)
            };

            copyButton.Click += CopyButton_Click;

            saveButton = new Button()
            {
                Text = "Save",
                Size = new Eto.Drawing.Size(100, 30)
            };

            saveButton.Click += SaveButton_Click;

            ftpsCertificateLocationButton = new Button()
            {
                Text = "...",
                Size = new Eto.Drawing.Size(35, 21)
            };

            ftpsCertificateLocationButton.Click += FtpsCertificateLocationButton_Click;

            keyPathButton = new Button()
            {
                Text = "...",
                Size = new Eto.Drawing.Size(35, 21)
            };

            keyPathButton.Click += KeyPathButton_Click;

            ftpProtocol.SelectedValueChanged += FtpProtocol_SelectedValueChanged;

            ftpsBoxes = GetFtpsBoxes();
            ftpsBoxes.Visible = false;

            sftpBoxes = GetSftpBoxes();
            sftpBoxes.Visible = false;

            ftpProtocol.SelectedIndex = 0;

            Content = new StackLayout
            {
                Orientation = Orientation.Vertical,
                Padding = 10,
                Items =
                {
                    GetAccountsPanel(),
                    new GroupBox()
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
                        MinimumSize = new Eto.Drawing.Size(550, 400),
                        Padding = 20
                    },
                    saveButton
                }
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
                    GetBaseStack("Name", name),
                    GetBaseStack("Ftp Protocol", ftpProtocol),
                    GetAdressBoxes(),
                    GetBaseStack("Username", username),
                    GetBaseStack("Password", password),
                    GetBaseStack("IsActive", isActive),
                    GetBaseStack("SubFolderPath", subFolderPath),
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
                Orientation = Orientation.Horizontal,
                Items =
                {
                    new Label()
                    {
                        Text = "Accounts:"
                    },
                    accounts,
                    addButton,
                    delButton,
                    copyButton
                }
            };
        }

        private StackLayout GetBaseStack(string name, Control control)
        {
            return new StackLayout
            {
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
                                Text = name
                            }
                        }
                    },
                    new StackLayout()
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Width = 300,
                        Items =
                        {
                            control
                        }
                    },
                }
            };
        }

        private GroupBox GetFtpsBoxes()
        {
            return new GroupBox()
            {
                Text = "Ftps",
                Width = 500,
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Items =
                    {
                        GetBaseStack("Encryption", ftpsEncryption),
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
                                            Text = "Location of the certificate"
                                        }
                                    }
                                },
                                new StackLayout()
                                {
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Orientation = Orientation.Horizontal,
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
                Text = "Sftp",
                Content = new StackLayout
                {
                    Width = 500,
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
                                            Text = "Location of the key:"
                                        }
                                    }
                                },
                                new StackLayout()
                                {
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    Orientation = Orientation.Horizontal,
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
                                GetBaseStack("Pass phrase", passphrase),
                            }
                        }
                    }
                }
            };
        }

        private StackLayout GetAdressBoxes()
        {
            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Padding = 5,
                Items =
                {
                    new Label()
                    {
                        Text = "Host:"
                    },
                    host,
                    new Label()
                    {
                        Text = "Port:"
                    },
                    port,
                }
            };
        }
    }
}
