using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions
{
    /// <summary>
    /// Codec options base dialog.
    /// </summary>
    internal abstract class CodecOptionsBase : Dialog
    {
        protected TableLayout layout;

        /// <summary>
        /// FFmpeg options.
        /// </summary>
        protected FFmpegOptions options;

        /// <summary>
        /// About message.
        /// </summary>
        protected readonly string aboutMessage;

        /// <summary>
        /// Quality about.
        /// </summary>
        private DefaultButton aboutButton;

        /// <summary>
        /// OK button.
        /// </summary>
        private DefaultButton okButton;

        /// <summary>
        /// Initializes codec options base.
        /// </summary>
        public CodecOptionsBase(FFmpegOptions options, string title, string about)
        {
            Title = title;
            this.aboutMessage = about;
            this.options = options;
            InitializeBaseComponents();
            InitializeComponents();
            AddButtons();

            Content = layout;
            Bind();
        }

        private void InitializeBaseComponents()
        {
            okButton = new DefaultButton("OK", 70, 25);
            okButton.Clicked += (o, e) => Close();

            aboutButton = new DefaultButton("About", 70, 25);
            aboutButton.Clicked += QualityAboutClicked;

            layout = new TableLayout
            {
                Padding = new Padding(15, 15, 15, 5),
                Spacing = new Size(5, 15),
                Width = 300
            };
        }

        /// <summary>
        /// Initializes components.
        /// </summary>
        protected abstract void InitializeComponents();

        private void AddButtons()
        {
            var row = new TableRow(
                null,
                TableLayout.AutoSized(
                new StackLayout()
                {
                    MinimumSize = new Size(200, 30),
                    Padding = new Padding(0, 5, 0, 0),
                    Orientation = Orientation.Vertical,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Items =
                    {
                        new StackLayout()
                        {
                            Orientation = Orientation.Horizontal,
                            VerticalContentAlignment = VerticalAlignment.Bottom,
                            Items =
                            {
                                okButton,
                                FormsHelper.GetVoidBox(5),
                                aboutButton
                            }
                        }
                    }
                }));

            layout.Rows.Add(row);
        }

        protected void AddQualityRow(string title, Control control)
        {
            var row = new TableRow(TableLayout.AutoSized(new Label { Text = title }, new Padding(0, 2)), control);
            layout.Rows.Add(row);
        }

        /// <summary>
        /// Handles quality about click event.
        /// </summary>
        protected virtual void QualityAboutClicked(object sender, EventArgs e)
        {
            MessageBox.Show(aboutMessage, "About", MessageBoxType.Information);
        }

        /// <summary>
        /// Binds controls with data context.
        /// </summary>
        protected abstract void Bind();
    }
}
