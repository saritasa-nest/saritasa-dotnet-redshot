using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.DataTransfer.Ftp;

namespace RedShot.Infrastructure.Uploaders.Ftp.Settings
{
    /// <summary>
    /// FTP option dialog.
    /// </summary>
    internal partial class FtpOptionControl : Panel
    {
        private Button addButton;
        private Button delButton;
        private Button copyButton;
        private ComboBox accounts;
        private TextBox name;
        private ComboBox ftpProtocol;
        private TextBox host;
        private NumericStepper port;
        private TextBox username;
        private PasswordBox password;
        private CheckBox isActive = new CheckBox();
        private TextBox subFolderPath;
        private ComboBox ftpsEncryption;
        private TextBox ftpsCertificateLocation;
        private TextBox keypath;
        private PasswordBox passphrase;
        private Button ftpsCertificateLocationButton;
        private Button keyPathButton;
        private Control ftpsBoxes;
        private Control sftpBoxes;
        private Control accountFields;
        private ObservableCollection<FtpAccount> bindingList;

        private readonly List<FtpAccount> ftpAccounts;

        /// <summary>
        /// Initializes FTP option dialog.
        /// </summary>
        public FtpOptionControl(FtpConfiguration ftpConfiguration)
        {
            ftpAccounts = ftpConfiguration.FtpAccounts;

            InitializeComponents();

            this.accounts.SelectedValueChanged += Accounts_SelectedValueChanged;
            this.accounts.DropDownClosed += Accounts_SelectedValueChanged;

            bindingList = new ObservableCollection<FtpAccount>(ftpAccounts);
            bindingList.CollectionChanged += BindingList_CollectionChanged;
            accounts.Bind(a => a.DataStore, bindingList, b => b);
        }

        private void BindingList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ftpAccounts.Clear();
            ftpAccounts.AddRange(bindingList);
        }

        private void Accounts_SelectedValueChanged(object sender, EventArgs e)
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

            name.Bind(t => t.Text, selectedAccount, account => account.Name);
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
        }

        private void KeyPathButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Key path";

                if (dialog.ShowDialog(this) == DialogResult.Ok)
                {
                    keypath.Text = dialog.FileName;
                }
            }
        }

        private void FtpsCertificateLocationButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "FTPS certificate";
                dialog.Filters.Add(new FileFilter("*.cer format", ".cer"));

                if (dialog.ShowDialog(this) == DialogResult.Ok)
                {
                    ftpsCertificateLocation.Text = dialog.FileName;
                }
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;
                CreateNewAccount(acc);
            }
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            if (accounts.DataStore.Count() > 0 && accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;

                bindingList.Remove(acc);

                accounts.SelectedIndex = -1;
                RefreshAccountFields();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
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
                newAccount = new FtpAccount();
            }

            bindingList.Add(newAccount);
            accounts.SelectedValue = newAccount;
        }
    }
}
