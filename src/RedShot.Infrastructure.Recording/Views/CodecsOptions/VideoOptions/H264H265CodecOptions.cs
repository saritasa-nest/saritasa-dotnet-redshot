using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;
using System;
using System.Linq;

namespace RedShot.Recording.Views.CodecsOptions.VideoOptions
{
    internal class H264H265CodecOptions : Dialog
    {
        private FFmpegOptions options;

        private ComboBox x264Preset;
        private NumericStepper x264Quality;
        private DefaultButton qualityAbout;

        private DefaultButton okButton;

        public H264H265CodecOptions(FFmpegOptions options)
        {
            Title = "H264 / H265 options";
            this.options = options;
            InitializeComponents();
            Bind();
        }

        private void InitializeComponents()
        {
            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += (o, e) => Close();

            x264Preset = new ComboBox()
            {
                Size = new Size(200, 21)
            };

            x264Quality = new NumericStepper()
            {
                Height = 21,
                MaxValue = 51,
                MinValue = 0,
                Increment = 1
            };

            qualityAbout = new DefaultButton("About", 60, 25);
            qualityAbout.Clicked += QualityAbout_Clicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 10,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                {
                    FormsHelper.GetBaseStack("Preset:", x264Preset, 50, 200),
                    FormsHelper.VoidBox(10),
                    FormsHelper.GetBaseStack("Quality:", GetQualityField(), 50, 250),
                    FormsHelper.VoidBox(20),
                    okButton,
                    FormsHelper.VoidBox(10)
                }
            };
        }

        private void QualityAbout_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show("The range of the CRF scale is 0–51, where 0 is lossless, 23 is the default, and 51 is worst quality possible.\n" +
                "A lower value generally leads to higher quality, and a subjectively sane range is 17–28.\n" +
                "Consider 17 or 18 to be visually lossless or nearly so;\n" +
                "it should look the same or nearly the same as the input but it isn't technically lossless.", MessageBoxType.Information);
        }

        private void Bind()
        {
            Content.DataContext = options;

            x264Preset.DataStore = EnumDescription<FFmpegX264Preset>.GetEnumDescriptions(typeof(FFmpegX264Preset));
            x264Preset.SelectedValueBinding.Convert(
                l =>
                {
                    if (l == null)
                    {
                        return FFmpegX264Preset.medium;
                    }
                    else
                    {
                        var des = (EnumDescription<FFmpegX264Preset>)l;
                        return des.EnumValue;
                    }
                },
                v =>
                {
                    return x264Preset.DataStore.FirstOrDefault(o => ((EnumDescription<FFmpegX264Preset>)o).EnumValue == v);
                }).BindDataContext((FFmpegOptions o) => o.X264Preset);

            x264Quality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.X264Crf);
        }

        private Control GetQualityField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    x264Quality,
                    FormsHelper.VoidBox(10),
                    qualityAbout
                }
            };
        }
    }
}
