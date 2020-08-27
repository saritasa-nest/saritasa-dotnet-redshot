using System;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.VideoOptions
{
    /// <summary>
    /// MPEG-4 options view.
    /// </summary>
    internal class MpegCodecOptions : Dialog
    {
        private FFmpegOptions options;
        private NumericStepper mpeg4Quality;
        private DefaultButton qualityAbout;
        private DefaultButton okButton;

        /// <summary>
        /// Initializes MPEG-4 options view.
        /// </summary>
        public MpegCodecOptions(FFmpegOptions options)
        {
            Title = "MPEG-4 options";
            this.options = options;
            InitializeComponents();
            Bind();
        }

        private void InitializeComponents()
        {
            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += (o, e) => Close();

            mpeg4Quality = new NumericStepper()
            {
                Height = 21,
                MaxValue = 31,
                MinValue = 1,
                Increment = 1
            };

            qualityAbout = new DefaultButton("About", 60, 25);
            qualityAbout.Clicked += QualityAbout_Clicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Items =
                {
                    FormsHelper.GetBaseStack("Quality:", GetQualityField(), 50, 250),
                    FormsHelper.VoidBox(20),
                    okButton,
                    FormsHelper.VoidBox(10)
                }
            };
        }

        private void QualityAbout_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show("The range of the quality level is 1–31, where 1 is highest quality/largest filesize and 31 is the lowest quality/smallest filesize", MessageBoxType.Information);
        }

        private void Bind()
        {
            Content.DataContext = options;

            mpeg4Quality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.XviDQscale);
        }

        private Control GetQualityField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    mpeg4Quality,
                    FormsHelper.VoidBox(10),
                    qualityAbout
                }
            };
        }
    }
}
