using Microsoft.Toolkit.Mvvm.Input;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Configuration.Models.Account;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.Domain.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Mvvm.ViewModels.Screenshots
{
    public class ScreenshotViewModel : BaseViewModel
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly File screenshot;

        public RelayCommand UploadImageToClipboardCommand { get; }

        public RelayCommand SaveImageToFileCommand { get; }

        public RelayCommand<FtpAccount> UploadImageToFtpCommand { get; }

        public bool HasImageChanged { get; set; }

        public AccountData PrimaryFtpAccount { get; private set; }

        public IEnumerable<AccountData> FtpAccounts { get; private set; }

        public ScreenshotViewModel(
            IConfigurationProvider configurationProvider, 
            File screenshot)
        {
            this.configurationProvider = configurationProvider;
        }

        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            LoadFtpAccounts();
        }

        private void LoadFtpAccounts()
        {
            var configuration = configurationProvider.GetConfiguration<AccountConfiguration>();
            FtpAccounts = configuration.Accounts;
            
            if (configuration.PrimaryAccountGuid != null)
            {
                PrimaryFtpAccount = FtpAccounts.First(a => a.Id == configuration.PrimaryAccountGuid);
            }
        }
    }
}
