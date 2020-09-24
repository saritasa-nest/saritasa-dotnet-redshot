using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Uploading.Settings
{
    public class UploadingSettingsSection : ISettingsSection
    {
        private readonly UploadingConfiguration uploadingConfiguration;
        private Control uploadingOptionControl;

        /// <summary>
        /// Initializes recording settings.
        /// </summary>
        public UploadingSettingsSection()
        {
            uploadingConfiguration = ConfigurationManager.GetSection<UploadingConfiguration>();
        }

        /// <inheritdoc/>
        public string Name => "Uploading options";

        /// <inheritdoc/>
        public Control GetControl()
        {
            if (uploadingOptionControl == null)
            {
                uploadingOptionControl = new UploadingOptionControl(uploadingConfiguration);
            }

            return uploadingOptionControl;
        }

        /// <inheritdoc/>
        public void Save()
        {
            ConfigurationManager.SetSettingsValue(uploadingConfiguration);
            ConfigurationManager.Save();
        }
    }
}
