using System;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// Provides options for audio codec.
    /// </summary>
    internal abstract class AudioCodecOptionsBase<T> : Dialog where T : Control
    {
        /// <summary>
        /// FFmpeg options.
        /// </summary>
        protected FFmpegOptions options;

        /// <summary>
        /// About message.
        /// </summary>
        protected readonly string aboutMessage;

        /// <summary>
        /// Code quality control.
        /// </summary>
        protected T codecQuality;

        /// <summary>
        /// Quality about.
        /// </summary>
        protected DefaultButton qualityAbout;

        /// <summary>
        /// OK button.
        /// </summary>
        protected DefaultButton okButton;

        /// <summary>
        /// Initializes the audio codec's options.
        /// </summary>
        public AudioCodecOptionsBase(FFmpegOptions options, string title, string about)
        {
            Title = title;
            this.aboutMessage = about;
            this.options = options;
            InitializeComponents();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Spacing = 10,
                Items =
                {
                    FormsHelper.GetBaseStack("Quality:", GetQualityField(), 50, 250),
                    FormsHelper.GetVoidBox(10),
                    okButton
                }
            };

            Bind();
        }

        /// <summary>
        /// Initializes components.
        /// </summary>
        protected virtual void InitializeComponents()
        {
            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += (o, e) => Close();

            qualityAbout = new DefaultButton("About", 60, 25);
            qualityAbout.Clicked += QualityAbout_Clicked;
        }

        /// <summary>
        /// Handles quality about click event.
        /// </summary>
        protected virtual void QualityAbout_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show(aboutMessage);
        }

        /// <summary>
        /// Binds controls with data context.
        /// </summary>
        protected abstract void Bind();

        /// <summary>
        /// Returns field with quality options.
        /// </summary>
        protected Control GetQualityField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 10,
                Items =
                {
                    codecQuality,
                    qualityAbout
                }
            };
        }
    }
}
