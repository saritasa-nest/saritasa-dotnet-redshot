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
        private readonly FormatConfigurationOption configurationOption;

        /// <summary>
        /// Initialize.
        /// </summary>
        public GeneralSettingsSection()
        {
            configurationOption = ConfigurationManager.GetSection<FormatConfigurationOption>();
        }

        /// <inheritdoc />
        public string Name => "General";

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
