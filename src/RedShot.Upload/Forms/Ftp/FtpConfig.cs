using Eto.Forms;
using RedShot.Configuration;
using RedShot.Helpers.FtpModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Upload.Forms.Ftp
{
    internal partial class FtpConfig : Dialog
    {
        private List<FtpAccount> ftpAccounts => ConfigurationManager.YamlConfig.FtpAccounts;
        public FtpConfig()
        {
            InitializeComponents();

            Title = "FTP/FTPS/SFTP Configuration";
            this.accounts.SelectedValueChanged += Accounts_SelectedValueChanged;
        }

        private void Accounts_SelectedValueChanged(object sender, EventArgs e)
        {
            if (accounts.DataStore.Count() > 0 && accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;
                FillData(acc);
            }
        }

        private void ClearBoxes()
        {
            name.Text = string.Empty;
            ftpProtocol.SelectedIndex = 0;
            host.Text = string.Empty;
            port.Value = 21;
            username.Text = string.Empty;
            password.Text = string.Empty;
            isActive.Checked = false;
            subFolderPath.Text = string.Empty;
            ftpsEncryption.SelectedIndex = 0;
            ftpsCertificateLocation.Text = string.Empty;
            keypath.Text = string.Empty;
            passphrase.Text = string.Empty;
        }

        private void FillData(FtpAccount account)
        {
            name.Text = account.Name;
            ftpProtocol.SelectedValue = account.Protocol.ToString();
            host.Text = account.Host;
            port.Value = account.Port;
            username.Text = account.Username;
            password.Text = account.Password;
            isActive.Checked = account.IsActive;
            subFolderPath.Text = account.SubFolderPath;
            ftpsEncryption.SelectedValue = account.FTPSEncryption.ToString();
            ftpsCertificateLocation.Text = account.FTPSCertificateLocation;
            keypath.Text = account.Keypath;
            passphrase.Text = account.Passphrase;
        }

        private FtpAccount GetAccountFromBoxes()
        {
            var account = new FtpAccount();

            account.Name = name.Text;
            account.Protocol = (FtpProtocol)Enum.Parse(typeof(FtpProtocol), (string)ftpProtocol.SelectedValue);
            account.Host = host.Text;
            account.Port = Convert.ToInt32(port.Value);
            account.Username = username.Text;
            account.Password = password.Text;
            account.IsActive = isActive.Checked ?? false;
            account.SubFolderPath = subFolderPath.Text;
            account.FTPSEncryption = (FtpsEncryption)Enum.Parse(typeof(FtpsEncryption), (string)ftpsEncryption.SelectedValue);
            account.FTPSCertificateLocation = ftpsCertificateLocation.Text;
            account.Keypath = keypath.Text;
            account.Passphrase = passphrase.Text;

            return account;
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveAccount();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            var acc = (FtpAccount)accounts.SelectedValue;
            CreateNewAccount(acc);
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            if (accounts.DataStore.Count() > 0 && accounts.SelectedValue != null)
            {
                var acc = (FtpAccount)accounts.SelectedValue;

                ftpAccounts.Remove(acc);

                if (ftpAccounts.Count != 0)
                {
                    accounts.SelectedIndex = -1;
                    accounts.DataStore = ftpAccounts;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            CreateNewAccount();
        }
        
        private void CreateNewAccount(FtpAccount account = null)
        {
            ClearBoxes();

            var acc = new FtpAccount();

            ftpAccounts.Add(acc);
            accounts.DataStore = ftpAccounts;
            accounts.SelectedValue = acc;

            if (account != null)
            {
                FillData(account);
            }
            else
            {
                FillData(acc);
            }
        }

        private void SaveAccount()
        {
            if (accounts.SelectedValue != null)
            {
                if (accounts.SelectedValue is FtpAccount account)
                {
                    var changedAcc = GetAccountFromBoxes();
                    changedAcc.Id = account.Id;

                    ftpAccounts.Remove(ftpAccounts.Where(a => a.Id == account.Id).FirstOrDefault());
                    ftpAccounts.Add(changedAcc);

                    accounts.DataStore = ftpAccounts;
                    accounts.SelectedValue = changedAcc;
                }
            }
        }
    }
}
