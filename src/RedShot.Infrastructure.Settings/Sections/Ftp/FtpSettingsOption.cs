using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Configuration.Models.Account;

namespace RedShot.Infrastructure.Settings.Sections.Ftp
{
    /// <summary>
    /// FTP setting's option.
    /// </summary>
    public sealed class FtpSettingsSection : ISettingsSection
    {
        private readonly FtpOptionControl ftpOptionControl;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FtpSettingsSection()
        {
            var accountConfiguration = ConfigurationProvider.Instance.GetConfiguration<AccountConfiguration>();
            var ftpConfiguration = Common.Mapping.Mapper.Map<FtpOptions>(accountConfiguration);
            ftpOptionControl = new FtpOptionControl(ftpConfiguration);
        }

        /// <inheritdoc />
        public string Name => "Accounts";

        /// <inheritdoc />
        public Control GetControl() => ftpOptionControl;

        /// <inheritdoc />
        public void Save()
        {
            var configuration = Common.Mapping.Mapper.Map<AccountConfiguration>(ftpOptionControl.FtpOptions);
            ConfigurationProvider.Instance.SetConfiguration(configuration);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ftpOptionControl.Dispose();
        }

        /// <summary>
        /// Returns name of the option.
        /// </summary>
        public override string ToString() => Name;
    }
}
