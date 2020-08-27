using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.RecordingRedShot.Settings;

namespace RedShot.Infrastructure.Recording.Settings
{
    /// <summary>
    /// Recording settings option.
    /// </summary>
    public class RecordingSettingsOption : ISettingsOption
    {
        private readonly FFmpegConfiguration ffmpegConfiguration;

        /// <summary>
        /// Initializes recording settings.
        /// </summary>
        public RecordingSettingsOption()
        {
            ffmpegConfiguration = ConfigurationManager.GetSection<FFmpegConfiguration>();
        }

        /// <inheritdoc/>
        public string Name => "FFmpeg recording options";

        /// <inheritdoc/>
        public Dialog<DialogResult> GetOptionDialog()
        {
            if (RecordingManager.CheckInstallFfmpeg())
            {
                return new RecordingOptionDialog(RecordingManager.RecordingService.GetRecordingDevices(), ffmpegConfiguration);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            ConfigurationManager.SetSettingsValue(ffmpegConfiguration);
            ConfigurationManager.Save();
        }

        /// Return name of the settings option.
        public override string ToString()
        {
            return Name;
        }
    }
}
