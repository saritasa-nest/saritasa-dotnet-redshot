using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings
{
    /// <summary>
    /// FTP setting's option.
    /// </summary>
    public class FtpSettingsSection : ISettingsSection
    {
        private readonly FtpConfiguration ftpConfiguration;
        private Control ftpOptionControl;

        /// <summary>
        /// Initializes FTP settings option.
        /// </summary>
        public FtpSettingsSection()
        {
            ftpConfiguration = ConfigurationManager.GetSection<FtpConfiguration>();
        }

        /// <inheritdoc />
        public string Name => "Accounts";

        /// <inheritdoc />
        public Control GetControl()
        {
            if (ftpOptionControl == null)
            {
                ftpOptionControl = new FtpOptionControl(ftpConfiguration);
            }

            return ftpOptionControl;
        }

        /// <inheritdoc />
        public void Save()
        {
            ConfigurationManager.SetSection(ftpConfiguration);
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
