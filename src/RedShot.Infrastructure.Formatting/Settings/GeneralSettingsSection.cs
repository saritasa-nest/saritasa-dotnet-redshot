using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// General settings option.
    /// </summary>
    public class GeneralSettingsSection : IValidatableSection
    {
        private Control formatOptionControl;
        private readonly GeneralConfigurationOption configurationOption;
        private readonly Autostart autostart;

        /// <summary>
        /// Initialize.
        /// </summary>
        public GeneralSettingsSection()
        {
            configurationOption = ConfigurationManager.GetSection<GeneralConfigurationOption>();
            autostart = new Autostart(configurationOption);
        }

        /// <inheritdoc />
        public string Name => "General";

        /// <inheritdoc />
        public Control GetControl()
        {
            if (formatOptionControl == null)
            {
                formatOptionControl = new GeneralOptionControl(configurationOption);
            }

            return formatOptionControl;
        }

        /// <inheritdoc />
        public void Save()
        {
            autostart.LaunchAtSystemStart = configurationOption.LaunchAtSystemStart.Value;
            ConfigurationManager.SetSettingsValue(configurationOption);
            ConfigurationManager.Save();
        }

        /// <inheritdoc />
        public ValidationResult Validate()
        {
            return FormattingValidator.Validate(configurationOption.Pattern);
        }
    }
}
