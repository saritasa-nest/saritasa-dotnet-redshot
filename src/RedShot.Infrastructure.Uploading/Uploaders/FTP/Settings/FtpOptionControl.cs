using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private TextBox previewLink;
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
        private TextBox directoryPath;
        private ComboBox ftpsEncryption;
        private TextBox ftpsCertificateLocation;
        private TextBox keypath;
        private PasswordBox passphrase;
        private DefaultButton ftpsCertificateLocationButton;
        private DefaultButton keyPathButton;
        private Control ftpsBoxes;
        private Control sftpBoxes;
        private Control accountFields;
        private CheckBox defaultAccountCheckBox;
        private ObservableCollection<FtpAccount> bindingList;
        private DefaultButton testButton;
        private readonly List<FtpAccount> ftpAccounts;
        private readonly FtpConfiguration ftpConfiguration;

        /// <summary>
        /// Active configuration data.
        /// </summary>
        public FtpConfiguration FtpConfiguration => ftpConfiguration;

        /// <summary>
        /// Initializes FTP option dialog.
        /// </summary>
        public FtpOptionControl(FtpConfiguration ftpConfiguration)
        {
            this.ftpConfiguration = ftpConfiguration;
            ftpAccounts = ftpConfiguration.FtpAccounts;

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

        private async void TestButtonClicked(object sender, EventArgs e)
        {
            if (SelectedAccount != null)
            {
                var ftpManager = new FtpUploadingService();
                var uploader = ftpManager.GetFtpUploader(SelectedAccount);
                var result = await uploader.TestConnectionAsync();

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
                previewLink.Text = account.GetFormatLink("example.png");
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
            if (accounts.SelectedValue == null)
            {
                DisableAccountFields();
            }
            else
            {
                EnableAccountFields();
            }

            BindBoxes();
        }

        private void BindBoxes()
        {
            var selectedAccount = (FtpAccount)accounts.SelectedValue;

            accountFields.Unbind();

            ftpProtocol.DataContext = selectedAccount;
            ftpProtocol.SelectedValueBinding.Convert(l => l != null ? Enum.Parse(typeof(FtpProtocol), (string) l) : null,
                v => v?.ToString() ?? FtpProtocol.FTP.ToString())
                .BindDataContext((FtpAccount m) => m.Protocol);
            host.Bind(t => t.Text, selectedAccount, account => account.Host);
            port.DataContext = selectedAccount;
            port.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FtpAccount m) => m.Port);
            username.Bind(t => t.Text, selectedAccount, account => account.Username);
            password.Bind(t => t.Text, selectedAccount, account => account.Password);
            isActive.Bind(t => t.Checked, selectedAccount, account => account.IsActive);
            directoryPath.Bind(t => t.Text, selectedAccount, account => account.Directory);
            ftpsEncryption.DataContext = selectedAccount;
            ftpsEncryption.SelectedValueBinding.Convert(l => l != null ? Enum.Parse(typeof(FtpsEncryption), (string)l) : null,
                    v => v?.ToString() ?? FtpsEncryption.Explicit.ToString())
                .BindDataContext((FtpAccount m) => m.FTPSEncryption);
            ftpsCertificateLocation.Bind(t => t.Text, selectedAccount, account => account.FTPSCertificateLocation);
            keypath.Bind(t => t.Text, selectedAccount, account => account.Keypath);
            passphrase.Bind(t => t.Text, selectedAccount, account => account.Passphrase);

            addExtensionCheckBox.Bind(t => t.Checked, selectedAccount, account => account.HttpHomePathAddExtension).Changed += FtpOptionControlChanged;
            homePathTextBox.Bind(t => t.Text, selectedAccount, account => account.HttpHomePath).Changed += FtpOptionControlChanged;

            defaultAccountCheckBox.DataContext = ftpConfiguration;
            defaultAccountCheckBox.CheckedBinding.Convert(DefaultAccountUpdated,
                v => selectedAccount != null && selectedAccount.Id == v)
                .BindDataContext((FtpConfiguration c) => c.PrimaryAccountGuid);
        }

        private Guid DefaultAccountUpdated(bool? isChecked)
        {
            if (!isChecked.HasValue || !isChecked.Value)
            {
                return default;
            }

            var selectedAccount = (FtpAccount)accounts.SelectedValue;
            return selectedAccount?.Id ?? default;
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

                if (ftpAccounts.Count == 1)
                {
                    ftpConfiguration.PrimaryAccountGuid = ftpAccounts.First().Id;
                }

                RefreshAccountFields();
            }
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            CreateNewAccount();
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
                newAccount = new FtpAccount
                {
                    HttpHomePathAddExtension = true
                };
            }

            if (ftpAccounts.Count == 0)
            {
                ftpConfiguration.PrimaryAccountGuid = newAccount.Id;
            }

            bindingList.Add(newAccount);
            accounts.SelectedIndex = accounts.DataStore.Count() - 1;
        }

        private void EnableAccountFields()
        {
            accountFields.Enabled = true;
            // This is necessary because simply disabling the ComboBox doesn't work for scrolling
            ftpProtocol.DataStore = Enum.GetValues(typeof(FtpProtocol)).Cast<FtpProtocol>()
                .Select(p => p.ToString());
            ftpsEncryption.DataStore = Enum.GetValues(typeof(FtpsEncryption)).Cast<FtpsEncryption>()
                .Select(p => p.ToString());
        }

        private void DisableAccountFields()
        {
            accountFields.Enabled = false;
            // This is necessary because simply disabling the ComboBox doesn't work for scrolling
            ftpProtocol.DataStore = Enumerable.Empty<string>();
            ftpsEncryption.DataStore = Enumerable.Empty<string>();
        }
    }
}
