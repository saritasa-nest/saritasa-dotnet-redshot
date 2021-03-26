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
    public sealed class RecordingSettingsSection : IValidatableSection
    {
        private readonly RecordingOptionsControl recordingOptionsControl;

        /// <summary>
        /// Initializes recording settings.
        /// </summary>
        public RecordingSettingsSection()
        {
            var configuration = ConfigurationProvider.Instance.GetConfiguration<RecordingConfiguration>();
            var recordingOptions = Mapping.Mapper.Map<RecordingOptions>(configuration);
            recordingOptionsControl = new RecordingOptionsControl(recordingOptions);
        }

        /// <inheritdoc/>
        public string Name => "Recording";

        /// <inheritdoc/>
        public void Dispose()
        {
            recordingOptionsControl.Dispose();
        }

        /// <inheritdoc/>
        public Control GetControl() => recordingOptionsControl;

        /// <inheritdoc/>
        public void Save()
        {
            var recordingOptions = recordingOptionsControl.RecordingOptions;
            var configuration = Mapping.Mapper.Map<RecordingConfiguration>(recordingOptions);
            ConfigurationProvider.Instance.SetConfiguration(configuration);
        }

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            return FFmpegOptionsValidator.Validate(recordingOptionsControl.RecordingOptions.FFmpegOptions);
        }
    }
}
