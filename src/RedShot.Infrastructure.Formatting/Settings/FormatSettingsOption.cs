using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format settings option.
    /// </summary>
    public class FormatSettingsOption : ISettingsOption
    {
        private readonly FormatConfigurationOption configurationOption =
            ConfigurationManager.GetSection<FormatConfigurationOption>();

        /// <inheritdoc />
        public string Name => "Format link";

        /// <inheritdoc />
        public Dialog<DialogResult> GetOptionDialog()
        {
            return new FormatOptionDialog(configurationOption);
        }

        /// <inheritdoc />
        public void Save()
        {
            ConfigurationManager.SetSettingsValue(configurationOption);
            ConfigurationManager.Save();
        }
    }
}
