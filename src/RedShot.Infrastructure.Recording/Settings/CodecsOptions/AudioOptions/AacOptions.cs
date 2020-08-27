﻿using Eto.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// AAC options view.
    /// </summary>
    internal class AacOptions : AudioCodecOptionsBase<NumericStepper>
    {
        private static readonly string about = "The range of the quality is 0-9 where a lower value is a higher quality. " +
            "0-3 will normally produce transparent results, 4 (default) should be close to perceptual transparency, and 6 produces an acceptable quality.";

        /// <inheritdoc/>
        public AacOptions(FFmpegOptions options) : base(options, "AAC options", about)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            codecQuality = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 9,
                Increment = 1
            };

            base.InitializeComponents();
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.AacQScale);
        }
    }
}
