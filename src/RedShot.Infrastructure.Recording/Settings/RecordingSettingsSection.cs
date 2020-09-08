﻿using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Validation;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Recording.Settings
{
    /// <summary>
    /// Recording settings option.
    /// </summary>
    public class RecordingSettingsSection : IValidatableSection
    {
        private readonly FFmpegConfiguration ffmpegConfiguration;
        private Control recordingOptionControl;

        /// <summary>
        /// Initializes recording settings.
        /// </summary>
        public RecordingSettingsSection()
        {
            ffmpegConfiguration = ConfigurationManager.GetSection<FFmpegConfiguration>();
        }

        /// <inheritdoc/>
        public string Name => "FFmpeg recording options";

        /// <inheritdoc/>
        public Control GetControl()
        {
            if (recordingOptionControl == null)
            {
                if (RecordingManager.CheckInstallFfmpeg())
                {
                    recordingOptionControl = new RecordingOptionControl(RecordingManager.RecordingService.GetRecordingDevices(), ffmpegConfiguration);
                }
            }

            return recordingOptionControl;
        }

        /// <inheritdoc/>
        public void Save()
        {
            ConfigurationManager.SetSettingsValue(ffmpegConfiguration);
            ConfigurationManager.Save();
        }

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            return FFmpegOptionsValidator.Validate(ffmpegConfiguration.Options);
        }
    }
}