using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings
{
    /// <summary>
    /// FTP option dialog.
    /// </summary>
    internal partial class FtpOptionControl : Panel
    {
        private FtpAccount SelectedAccount => accounts.SelectedValue as FtpAccount;

        private CheckBox addExtensionCheckBox;
        private Label previewLinkLabel;
        private TextBox browserTypeTextBox;
        private TextBox homePathTextBox;
        private Button addButton;
        private Button delButton;
        private Button copyButton;
        private ComboBox accounts;
        private ComboBox ftpProtocol;
        private TextBox host;
        private NumericStepper port;
        private TextBox username;
        private PasswordBox password;
        private CheckBox isActive;
        private TextBox subFolderPath;
        private ComboBox ftpsEncryption;
        private TextBox ftpsCertificateLocation;
        private TextBox keypath;
        private PasswordBox passphrase;
        private DefaultButton ftpsCertificateLocationButton;
        private DefaultButton keyPathButton;
        private Control ftpsBoxes;
        private Control sftpBoxes;
        private Control accountFields;
        private ObservableCollection<FtpAccount> bindingList;
        private DefaultButton testButton;
        private CheckBox defaultProtocolCheckBox;
        private CheckBox defaultHostCheckBox;
        private CheckBox defaultUserCheckBox;
        private CheckBox defaultDirectoryCheckBox;
        private CheckBox defaultBaseUrlCheckBox;
        private readonly List<FtpAccount> ftpAccounts;
        private readonly FtpAccount defaultFtpAccount;
        private readonly FtpConfiguration ftpConfiguration;

        /// <summary>
        /// Initializes FTP option dialog.
        /// </summary>
        public FtpOptionControl(FtpConfiguration ftpConfiguration)
        {
            this.ftpConfiguration = ftpConfiguration;
            ftpAccounts = ftpConfiguration.FtpAccounts;
            defaultFtpAccount = ftpConfiguration.DefaultFtpAccount ?? new FtpAccount();

            InitializeComponents();
            InitializeAccountsBinding();
        }

        private void InitializeAccountsBinding()
        {
            bindingList = new ObservableCollection<FtpAccount>(ftpAccounts);
            bindingList.CollectionChanged += BindingList_CollectionChanged;

            accounts.DropDownClosed += AccountsSelectedValueChanged;
            accounts.SelectedValueChanged += AccountsSelectedValueChanged;
            accounts.DataStore = bindingList;
            accounts.SelectedValueChanged += FtpOptionControlChanged;
            accounts.ItemKeyBinding = new DelegateBinding<FtpAccount, string>(a => a.ToString());
        }

        private void TestButtonClicked(object sender, EventArgs e)
        {
            if (SelectedAccount != null)
            {
                var ftpManager = new FtpUploadingService();
                var uploader = ftpManager.GetFtpUploader(SelectedAccount);
                var result = uploader.TestConnection();

                if (result)
                {
                    NotifyHelper.Notify("Connection to the FTP server was succeed!", "RedShot", NotifyStatus.Success);
                }
                else
                {
                    NotifyHelper.Notify("Connection to the FTP server was failed!", "RedShot", NotifyStatus.Failed);
                }
            }
        }

        private void UpdatePreview()
        {
            if (accounts.SelectedValue is FtpAccount account)
            {
                previewLinkLabel.Text = account.GetFormatLink("example.png");
            }
        }

        private void BindingList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ftpAccounts.Clear();
            ftpAccounts.AddRange(bindingList);
        }

        private void AccountsSelectedValueChanged(object sender, EventArgs e)
        {
            RefreshAccountFields();
        }

        private void RefreshAccountFields()
        {
            BindBoxes();

            if (accounts.SelectedValue == null)
            {
                accountFields.Enabled = false;
            }
            else
            {
                accountFields.Enabled = true;
            }
        }

        private void BindBoxes()
        {
            var selectedAccount = (FtpAccount)accounts.SelectedValue;

            accountFields.Unbind();

            ftpProtocol.DataContext = selectedAccount;
            ftpProtocol.SelectedValueBinding.Convert(l => Enum.Parse(typeof(FtpProtocol), (string)l), v => v?.ToString() ?? FtpProtocol.FTP.ToString())
                .BindDataContext((FtpAccount m) => m.Protocol);
            host.Bind(t => t.Text, selectedAccount, account => account.Host);
            port.DataContext = selectedAccount;
            port.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FtpAccount m) => m.Port);
            username.Bind(t => t.Text, selectedAccount, account => account.Username);
            password.Bind(t => t.Text, selectedAccount, account => account.Password);
            isActive.Bind(t => t.Checked, selectedAccount, account => account.IsActive);
            subFolderPath.Bind(t => t.Text, selectedAccount, account => account.SubFolderPath);
            ftpsEncryption.DataContext = selectedAccount;
            ftpsEncryption.SelectedValueBinding.Convert(l => Enum.Parse(typeof(FtpsEncryption), (string)l), v => v?.ToString() ?? FtpsEncryption.Explicit.ToString())
                .BindDataContext((FtpAccount m) => m.FTPSEncryption);
            ftpsCertificateLocation.Bind(t => t.Text, selectedAccount, account => account.FTPSCertificateLocation);
            keypath.Bind(t => t.Text, selectedAccount, account => account.Keypath);
            passphrase.Bind(t => t.Text, selectedAccount, account => account.Passphrase);

            addExtensionCheckBox.Bind(t => t.Checked, selectedAccount, account => account.HttpHomePathAddExtension).Changed += FtpOptionControlChanged;
            homePathTextBox.Bind(t => t.Text, selectedAccount, account => account.HttpHomePath).Changed += FtpOptionControlChanged;

            browserTypeTextBox.Bind(t => t.Text, selectedAccount, ac => ac.BrowserProtocol.ToString().ToLower())
                .Changed += (sender, args) =>
            {
                var text = FirstLetterToUpper(browserTypeTextBox.Text);
                if (Enum.TryParse<BrowserProtocol>(text, out var browserProtocol))
                {
                    selectedAccount.BrowserProtocol = browserProtocol;
                    UpdatePreview();
                }
            };
        }

        /// <inheritdoc/>
        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            ftpConfiguration.DefaultFtpAccount = CreateNewDefaultFtpAccount();
        }

        private FtpAccount CreateNewDefaultFtpAccount()
        {
            var newDefaultAccount = defaultFtpAccount.Clone();

            if (defaultProtocolCheckBox.Checked != null && defaultProtocolCheckBox.Checked.Value)
            {
                if (Enum.TryParse<FtpProtocol>(ftpProtocol.Text, out var protocol))
                {
                    newDefaultAccount.Protocol = protocol;
                }
            }

            if (defaultHostCheckBox.Checked != null && defaultHostCheckBox.Checked.Value)
            {
                newDefaultAccount.Host = host.Text;
                newDefaultAccount.Port = (int)port.Value;
            }

            if (defaultUserCheckBox.Checked != null && defaultUserCheckBox.Checked.Value)
            {
                newDefaultAccount.Username = username.Text;
                newDefaultAccount.Password = password.Text;
            }

            if (defaultBaseUrlCheckBox.Checked != null && defaultBaseUrlCheckBox.Checked.Value)
            {
                newDefaultAccount.HttpHomePath = homePathTextBox.Text;
            }

            if (defaultDirectoryCheckBox.Checked != null && defaultDirectoryCheckBox.Checked.Value)
            {
                newDefaultAccount.SubFolderPath = subFolderPath.Text;
            }

            return newDefaultAccount;
        }

        private void FtpOptionControlChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void KeyPathButtonClick(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Key path"
            };

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                keypath.Text = dialog.FileName;
            }
        }

        private void FtpsCertificateLocationButtonClick(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "FTPS certificate"
            };
            dialog.Filters.Add(new FileFilter("*.cer format", ".cer"));

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                ftpsCertificateLocation.Text = dialog.FileName;
            }
        }

        private void CopyButtonClick(object sender, EventArgs e)
        {
            if (accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;
                CreateNewAccount(acc);
            }
        }

        private void DelButtonClick(object sender, EventArgs e)
        {
            if (accounts.DataStore.Any() && accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;

                bindingList.Remove(acc);

                accounts.SelectedIndex = -1;
                if (bindingList.Count == 0)
                {
                    accounts.Text = string.Empty;
                }

                RefreshAccountFields();
            }
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            CreateNewAccount(defaultFtpAccount);
        }

        private void CreateNewAccount(FtpAccount account = null)
        {
            FtpAccount newAccount;

            if (account != null)
            {
                newAccount = account.Clone();
            }
            else
            {
                newAccount = new FtpAccount();
            }

            bindingList.Add(newAccount);
            accounts.SelectedIndex = accounts.DataStore.Count() - 1;
        }

        private string FirstLetterToUpper(string str)
        {
            if (browserTypeTextBox.Text.Length < 1)
            {
                return str;
            }
            var text = new StringBuilder(browserTypeTextBox.Text);
            var firstLetter = text[0];
            return text.Remove(0, 1)
                .Insert(0, firstLetter.ToString().ToUpper())
                .ToString();
        }
    }
}
