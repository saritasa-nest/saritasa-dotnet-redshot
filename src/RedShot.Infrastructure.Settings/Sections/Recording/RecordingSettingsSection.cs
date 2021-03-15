using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using RedShot.Infrastructure.Recording.Validation;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Infrastructure.Configuration.Models.Recording;

namespace RedShot.Infrastructure.Settings.Sections.Recording
{
    /// <summary>
    /// Recording settings option.
    /// </summary>
    public class RecordingSettingsSection : IValidatableSection
    {
        private readonly FFmpegConfigurationOption ffmpegConfiguration;
        private Control recordingOptionControl;

        /// <summary>
        /// Initializes recording settings.
        /// </summary>
        public RecordingSettingsSection()
        {
            var configurationModel = ConfigurationProvider.Instance.GetConfiguration<RecordingConfiguration>();
            ffmpegConfiguration = Mapping.Mapper.Map<FFmpegConfigurationOption>(configurationModel);
        }

        /// <inheritdoc/>
        public string Name => "Recording";

        /// <inheritdoc/>
        public Control GetControl()
        {
            if (recordingOptionControl == null)
            {
                recordingOptionControl = new RecordingOptionControl(ffmpegConfiguration);
            }

            return recordingOptionControl;
        }

        /// <inheritdoc/>
        public void Save()
        {
            var configurationModel = Mapping.Mapper.Map<RecordingConfiguration>(ffmpegConfiguration);
            ConfigurationProvider.Instance.SetConfiguration(configurationModel);
        }

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            return FFmpegOptionsValidator.Validate(ffmpegConfiguration.FFmpegOptions);
        }
    }
}
