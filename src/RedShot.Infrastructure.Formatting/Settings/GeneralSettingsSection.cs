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
        private Control generalOptionControl;
        private readonly GeneralConfigurationOption configurationOption;
        private readonly Autostart autostart;

        /// <summary>
        /// Initialize.
        /// </summary>
        public GeneralSettingsSection()
        {
            configurationOption = ConfigurationManager.GetSection<GeneralConfigurationOption>();
            autostart = new Autostart();
        }

        /// <inheritdoc />
        public string Name => "General";

        /// <inheritdoc />
        public Control GetControl()
        {
            if (generalOptionControl == null)
            {
                generalOptionControl = new GeneralOptionControl(configurationOption);
            }

            return generalOptionControl;
        }

        /// <inheritdoc />
        public void Save()
        {
            if (configurationOption.LaunchAtSystemStart)
            {
                autostart.EnableAutostart();
            }
            else
            {
                autostart.DisableAutostart();
            }

            ConfigurationManager.SetSection(configurationOption);
            ConfigurationManager.Save();
        }

        /// <inheritdoc />
        public ValidationResult Validate()
        {
            return FormattingValidator.Validate(configurationOption.Pattern);
        }
    }
}
