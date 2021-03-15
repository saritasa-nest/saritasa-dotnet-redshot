using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Infrastructure.Settings.Sections.Recording
{
    /// <summary>
    /// Codec options base dialog.
    /// </summary>
    internal abstract class CodecOptionsBase : Dialog
    {
        /// <summary>
        /// Control layout.
        /// </summary>
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
        protected DefaultButton aboutButton;

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
            AddButtonControl(aboutButton);
            AddButtonControl(okButton);

            Content = layout;
            Bind();
        }

        /// <summary>
        /// Initializes components.
        /// </summary>
        protected abstract void InitializeComponents();

        /// <summary>
        /// Add quality row option control.
        /// </summary>
        /// <param name="title">Name of the control.</param>
        /// <param name="control">Option control.</param>
        protected void AddQualityRow(string title, Control control)
        {
            var row = new TableRow(TableLayout.AutoSized(new Label { Text = title }, new Padding(0, 2)), control);
            layout.Rows.Add(row);
        }

        /// <summary>
        /// Add quality row with about button option control.
        /// </summary>
        /// <param name="title">Name of the control.</param>
        /// <param name="control">Option control.</param>
        protected void AddQualityRowWithAbout(string title, Control control)
        {
            AddQualityRow(title, TableLayout.AutoSized(TableLayout.HorizontalScaled(5, control, aboutButton)));
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

        private void AddButtonControl(Control control)
        {
            var row = new TableRow(
                null,
                new StackLayout()
                {
                    MinimumSize = new Size(200, 25),
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Items =
                    {
                        control
                    }
                });

            layout.Rows.Add(row);
        }
    }
}
