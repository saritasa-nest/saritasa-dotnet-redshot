using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.VideoOptions
{
    /// <summary>
    /// VP9 options view.
    /// </summary>
    internal class Vp9CodecOptions : Dialog
    {
        private FFmpegOptions options;
        private NumericStepper vp9Quality;
        private DefaultButton qualityAbout;
        private TextBox vp9Bitrate;
        private DefaultButton okButton;

        /// <summary>
        /// Initializes VP9 options view.
        /// </summary>
        public Vp9CodecOptions(FFmpegOptions options)
        {
            Title = "VP9 options";
            this.options = options;
            InitializeComponents();
            Bind();
        }

        private void InitializeComponents()
        {
            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += (o, e) => Close();

            vp9Bitrate = new TextBox()
            {
                Size = new Size(150, 21)
            };

            vp9Quality = new NumericStepper()
            {
                Height = 21,
                MaxValue = 63,
                MinValue = 0,
                Increment = 1
            };

            qualityAbout = new DefaultButton("About", 60, 25);
            qualityAbout.Clicked += QualityAbout_Clicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                Spacing = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Items =
                {
                    FormsHelper.GetBaseStack("Bitrate:", GetBitrateFiled(), 50, 250),
                    FormsHelper.GetBaseStack("Quality:", GetQualityField(), 50, 250),
                    okButton,
                }
            };
        }

        private Control GetBitrateFiled()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    vp9Bitrate,
                    new Label()
                    {
                        Text = "K"
                    }
                }
            };
        }

        private void QualityAbout_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show("The quality value can be from 0–63. Lower values mean better quality. " +
                "Recommended values range from 15–35, with 31 being recommended for 1080p HD video", MessageBoxType.Information);
        }

        private void Bind()
        {
            Content.DataContext = options;

            vp9Bitrate.TextBinding.Convert(
                l =>
                {
                    if (int.TryParse(l, out int result))
                    {
                        return result;
                    }
                    else
                    {
                        return 20;
                    }
                },
                v =>
                {
                    return v.ToString();
                }).BindDataContext((FFmpegOptions o) => o.Vp9Bitrate);

            vp9Quality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.Vp9Crf);
        }

        private Control GetQualityField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 10,
                Items =
                {
                    vp9Quality,
                    qualityAbout
                }
            };
        }
    }
}
