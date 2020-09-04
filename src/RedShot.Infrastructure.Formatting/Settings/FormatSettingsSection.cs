using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format settings option.
    /// </summary>
    public class FormatSettingsSection : ISettingsSection
    {
        private Control formatOptionControl;

        private readonly FormatConfigurationOption configurationOption;

        public FormatSettingsSection()
        {
            configurationOption = ConfigurationManager.GetSection<FormatConfigurationOption>();
        }

        /// <inheritdoc />
        public string Name => "Format link";

        /// <inheritdoc />
        public Control GetControl()
        {
            if (formatOptionControl == null)
            {
                formatOptionControl = new FormatOptionControl(configurationOption);
            }

            return formatOptionControl;
        }

        /// <inheritdoc />
        public void Save()
        {
            ConfigurationManager.SetSettingsValue(configurationOption);
            ConfigurationManager.Save();
        }
    }
}
