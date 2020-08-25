using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Uploaders.Ftp;
using RedShot.Infrastructure.Uploaders.Ftp.Settings;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings
{
    public class FtpSettingOption : ISettingsOption
    {
        private readonly FtpConfiguration ftpConfiguration;

        public FtpSettingOption()
        {
            ftpConfiguration = ConfigurationManager.GetSection<FtpConfiguration>();
        }

        public string Name => "FTP / FTPS / SFTP";

        public Control GetControl()
        {
            return new FtpSettingControl(ftpConfiguration);
        }

        public void Save()
        {
            ConfigurationManager.SetSettingsValue(ftpConfiguration);
            ConfigurationManager.Save();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
