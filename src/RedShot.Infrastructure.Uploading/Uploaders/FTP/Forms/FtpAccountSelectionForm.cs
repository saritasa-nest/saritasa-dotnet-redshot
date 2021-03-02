using System;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Configuration.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Forms
{
    /// <summary>
    /// FTP uploader form.
    /// </summary>
    internal class FtpAccountSelectionForm : Dialog<DialogResult>
    {
        private readonly FtpConfiguration ftpConfiguration;
        private ComboBox accounts;
        private DefaultButton selectionButton;

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount SelectedAccount { get; private set; }

        /// <summary>
        /// Initializes new FTP account selection form.
        /// </summary>
        public FtpAccountSelectionForm()
        {
            var accountConfig = ConfigurationProvider.Instance.GetConfiguration<AccountConfiguration>();
            ftpConfiguration = Mapping.Mapper.Map<FtpConfiguration>(accountConfig);
            Title = "FTP account selection";
            ShowInTaskbar = true;
            InitializeComponents();
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            accounts = new ComboBox()
            {
                DataStore = ftpConfiguration.FtpAccounts,
                Size = new Size(250, 21),
            };

            if (ftpConfiguration.FtpAccounts.Count > 0)
            {
                accounts.SelectedValue = ftpConfiguration.FtpAccounts.First();
            }

            selectionButton = new DefaultButton("Select", 120, 30);
            selectionButton.Clicked += SelectionButtonClick;

            Content = new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical,
                Padding = new Padding(15, 5),
                Spacing = 15,
                Items =
                {
                    GetAccountLayout(),
                    selectionButton
                }
            };
        }

        private StackLayout GetAccountLayout()
        {
            return new StackLayout()
            {
                Padding = 10,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical,
                Spacing = 10,
                Items =
                {
                    new Label()
                    {
                        Text = "Account:",
                        Width = 100,
                    },
                    accounts,
                }
            };
        }

        private void SelectionButtonClick(object sender, EventArgs e)
        {
            if (accounts.DataStore.Any() && accounts.SelectedValue != null)
            {
                SelectedAccount = (FtpAccount)accounts.SelectedValue;
                Result = DialogResult.Ok;
            }
            else
            {
                Result = DialogResult.Cancel;
            }

            Close();
        }
    }
}
