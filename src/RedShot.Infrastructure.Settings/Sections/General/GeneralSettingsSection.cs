using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Configuration.Models.General;
using RedShot.Infrastructure.Abstractions;

namespace RedShot.Infrastructure.Settings.Sections.General
{
    /// <summary>
    /// General settings option.
    /// </summary>
    public sealed class GeneralSettingsSection : IValidatableSection
    {
        private readonly IApplicationUpdateService applicationUpdateService;
        private readonly GeneralOptionsControl generalOptionsControl;
        private readonly Autostart autostart;

        /// <summary>
        /// Initialize.
        /// </summary>
        public GeneralSettingsSection()
        {
            applicationUpdateService = Updating.ApplicationUpdateService;
            var configurationModel = ConfigurationProvider.Instance.GetConfiguration<GeneralConfiguration>();
            var generalOptions = Mapping.Mapper.Map<GeneralOptions>(configurationModel);
            generalOptionsControl = new GeneralOptionsControl(generalOptions);
            autostart = new Autostart();
        }

        /// <inheritdoc />
        public string Name => "General";

        /// <inheritdoc />
        public void Dispose()
        {
            generalOptionsControl.Dispose();
        }

        /// <inheritdoc />
        public Control GetControl() => generalOptionsControl;

        /// <inheritdoc />
        public void Save()
        {
            var generalOptions = generalOptionsControl.GeneralOptions;
            var configuration = Mapping.Mapper.Map<GeneralConfiguration>(generalOptions);
            ConfigurationProvider.Instance.SetConfiguration(configuration);

            ApplySettings(generalOptions);
        }

        private void ApplySettings(GeneralOptions generalOptions)
        {
            applicationUpdateService.ChangeInterval(generalOptions.UpdateInterval);

            if (generalOptions.LaunchAtSystemStart)
            {
                autostart.EnableAutostart();
            }
            else
            {
                autostart.DisableAutostart();
            }
        }

        /// <inheritdoc />
        public ValidationResult Validate()
        {
            var generalOptions = generalOptionsControl.GeneralOptions;

            if (FormatManager.TryFormat(generalOptions.Pattern, out _))
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
