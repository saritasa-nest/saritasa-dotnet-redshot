using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Uploaders.Ftp;
using RedShot.Infrastructure.Uploaders.Ftp.Settings;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings
{
    /// <summary>
    /// FTP setting's option.
    /// </summary>
    public class FtpSettingsOption : ISettingsOption
    {
        private readonly FtpConfiguration ftpConfiguration;

        /// <summary>
        /// Initializes FTP settings option.
        /// </summary>
        public FtpSettingsOption()
        {
            ftpConfiguration = ConfigurationManager.GetSection<FtpConfiguration>();
        }

        /// <inheritdoc />
        public string Name => "FTP / FTPS / SFTP";

        /// <inheritdoc />
        public Dialog<DialogResult> GetOptionDialog()
        {
            return new FtpOptionDialog(ftpConfiguration);
        }

        /// <inheritdoc />
        public void Save()
        {
            ConfigurationManager.SetSettingsValue(ftpConfiguration);
            ConfigurationManager.Save();
        }

        /// <summary>
        /// Returns name of the option.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
