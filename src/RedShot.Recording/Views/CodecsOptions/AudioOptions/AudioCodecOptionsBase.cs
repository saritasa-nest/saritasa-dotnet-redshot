using System;
using Eto.Forms;
using RedShot.Helpers.Ffmpeg.Options;
using RedShot.Helpers.Forms;

namespace RedShot.Recording.Views.CodecsOptions.AudioOptions
{
    internal abstract class AudioCodecOptionsBase<T> : Dialog where T : Control
    {
        protected FFmpegOptions options;
        protected readonly string aboutMessage;
        protected T codecQuality;
        protected DefaultButton qualityAbout;
        protected DefaultButton okButton;

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
                Items =
                {
                    FormsHelper.GetBaseStack("Quality:", GetQualityField(), 50, 200),
                    FormsHelper.VoidBox(20),
                    okButton,
                    FormsHelper.VoidBox(10)
                }
            };

            Bind();
        }

        protected virtual void InitializeComponents()
        {
            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += (o, e) => Close();

            qualityAbout = new DefaultButton("About", 60, 25);
            qualityAbout.Clicked += QualityAbout_Clicked;
        }

        protected virtual void QualityAbout_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show(aboutMessage);
        }

        protected abstract void Bind();

        protected Control GetQualityField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    codecQuality,
                    FormsHelper.VoidBox(10),
                    qualityAbout
                }
            };
        }
    }
}
