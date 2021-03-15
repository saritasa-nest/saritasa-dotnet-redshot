using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Configuration.Models;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;

namespace RedShot.Infrastructure.Settings.Sections.Ftp
{
    /// <summary>
    /// FTP setting's option.
    /// </summary>
    public class FtpSettingsSection : ISettingsSection
    {
        private FtpOptionControl ftpOptionControl;

        /// <inheritdoc />
        public string Name => "Accounts";

        /// <inheritdoc />
        public Control GetControl()
        {
            if (ftpOptionControl == null)
            {
                var accountConfiguration = ConfigurationProvider.Instance.GetConfiguration<AccountConfiguration>();
                var ftpConfiguration = Common.Mapping.Mapper.Map<FtpConfiguration>(accountConfiguration);
                ftpOptionControl = new FtpOptionControl(ftpConfiguration);
            }

            return ftpOptionControl;
        }

        /// <inheritdoc />
        public void Save()
        {
            var configuration = Common.Mapping.Mapper.Map<AccountConfiguration>(ftpOptionControl.FtpConfiguration);
            ConfigurationProvider.Instance.SetConfiguration(configuration);
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
