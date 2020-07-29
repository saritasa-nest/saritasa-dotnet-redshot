using Eto.Forms;
using RedShot.Configuration;
using RedShot.Helpers.FtpModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Upload.Forms.Ftp
{
    public partial class FtpConfig : Form
    {
        private List<FtpAccount> ftpAccounts => ConfigurationManager.YamlConfig.FtpAccounts;

        private TextBox name = new TextBox();

        // FtpProtocol enum.
        private ComboBox ftpProtocol;
        private TextBox host = new TextBox();
        private NumericStepper port;
        private TextBox username = new TextBox();
        private MaskedTextBox password = new MaskedTextBox();
        private CheckBox isActive = new CheckBox();
        private TextBox subFolderPath = new TextBox();

        // BrowserProtocol enum.

        private ComboBox browserProtocol;
        private TextBox httpHomePath = new TextBox();
        private CheckBox httpHomePathAutoAddSubFolderPath = new CheckBox();
        private CheckBox httpHomePathNoExtension = new CheckBox();

        // ComboBox.
        private ComboBox ftpsEncryption;
        private TextBox ftpsCertificateLocation = new TextBox();
        private TextBox keypath = new TextBox();
        private MaskedTextBox passphrase = new MaskedTextBox();

        private void InitBoxes()
        {
            port = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 65535,
                Increment = 1,
                Value = 21
            };

            ftpProtocol = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpProtocol)).Cast<FtpProtocol>()
                .Select(p => p.ToString()),
            };

            browserProtocol = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(BrowserProtocol)).Cast<BrowserProtocol>()
                .Select(p => p.ToString()),
            };

            ftpsEncryption = new ComboBox()
            {
                DataStore = Enum.GetValues(typeof(FtpsEncryption)).Cast<FtpsEncryption>()
                .Select(p => p.ToString()),
            };
        }

        public FtpConfig()
        {
            InitBoxes();

            Content = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Items =
                {
                    BaseAccountBoxes()
                }
            };

        }

        private StackLayout BaseAccountBoxes()
        {
            return new StackLayout
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    GetBaseStack("Name", name),
                    GetBaseStack("ftpProtocol", ftpProtocol),
                    GetBaseStack("host", host),
                    GetBaseStack("port", port),
                    GetBaseStack("username", username),
                    GetBaseStack("password", password),
                    GetBaseStack("isActive", isActive),
                    GetBaseStack("subFolderPath", subFolderPath),
                    GetBaseStack("httpHomePath", httpHomePath)
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
                        Width = 100,
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
                        Width = 200,
                        Items =
                        {
                            control
                        }
                    },
                }
            };
        }       
    }
}
