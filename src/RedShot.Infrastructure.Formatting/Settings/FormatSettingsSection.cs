using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format settings option.
    /// </summary>
    public class FormatSettingsSection : IValidatableSection
    {
        private Control formatOptionControl;
        private readonly FormatConfigurationOption configurationOption;

        /// <summary>
        /// Initialize.
        /// </summary>
        public FormatSettingsSection()
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
