using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Formatting;

namespace RedShot.Infrastructure.Settings.Sections.General
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
            configurationOption = UserConfiguration.Instance.GetOptionOrDefault<GeneralConfigurationOption>();
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

            UserConfiguration.Instance.SetOption(configurationOption);
        }

        /// <inheritdoc />
        public ValidationResult Validate()
        {
            if (FormatManager.TryFormat(configurationOption.Pattern, out _))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid pattern in format link");
            }
        }
    }
}
