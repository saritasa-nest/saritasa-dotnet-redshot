using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Recording.Common.Views
{
    /// <summary>
    /// Recording view base.
    /// </summary>
    public partial class RecordingViewBase : Form
    {
        /// <summary>
        /// Initialize components.
        /// </summary>
        protected virtual void InitializeComponents()
        {
            InitializeOptionsPanel();

            Padding contentPadding = default;

            recordingTimer = new Stopwatch();
            renderingTimer = new UITimer
            {
                Interval = 0.01
            };
            renderingTimer.Elapsed += RenderingTimerElapsed;
            renderingTimer.Start();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = contentPadding,
                Items =
                {
                    optionsPanel
                }
            };
        }

        /// <summary>
        /// Initialize options panel.
        /// </summary>
        protected void InitializeOptionsPanel()
        {
            timerLabel = new Label()
            {
                Text = TimeSpan.Zero.ToString()
            };
            var buttonSize = new Size(40, 35);
            var scaleSize = new Size(20, 18);

            recordingButton = new RecordingButton(buttonSize);
            recordingButton.Clicked += RecordingButtonClicked;

            closeButton = new ImageButton(buttonSize, Icons.Close, scaleImageSize: scaleSize);
            closeButton.ToolTip = "Close";
            closeButton.Clicked += (o, e) => Close();

            optionsButton = new ImageButton(buttonSize, Icons.Gear, scaleImageSize: scaleSize);
            optionsButton.ToolTip = "Audio Options";
            optionsButton.Clicked += OptionsButtonClicked;

            var optionsPanelSize = new Size(220, 41);

            optionsPanel = new StackLayout()
            {
                Size = optionsPanelSize,
                BackgroundColor = Colors.WhiteSmoke,
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 3,
                Spacing = 5,
                Items =
                {
                    recordingButton,
                    timerLabel,
                    FormsHelper.GetVoidBox(15),
                    optionsButton,
                    closeButton
                }
            };
        }
    }
}
