using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Recording view.
    /// </summary>
    internal partial class RecordingView : Form
    {
        private void InitializeComponents()
        {
            InitializeOptionsPanel();

            Padding contentPadding = default;

#if _WINDOWS
            WindowStyle = WindowStyle.None;
            BackgroundColor = Colors.Red;

            Rectangle excludeRectangle = default;
            Rectangle excludeRectangle2 = default;

            if (CheckPanelFit())
            {
                Location = new Point(recordingRectangle.X, recordingRectangle.Y - optionsPanel.Height - 1);
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + optionsPanel.Height + 1);
                excludeRectangle = new Rectangle(new Point(0, optionsPanel.Height), new Size(recordingRectangle.Width, recordingRectangle.Height + 1)).OffsetRectangle(1);
                excludeRectangle2 = new Rectangle(new Point(optionsPanel.Width, 0), new Size(recordingRectangle.Width - optionsPanel.Width, optionsPanel.Height));
            }
            else
            {
                Location = recordingRectangle.Location;
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + 1);
                excludeRectangle = new Rectangle(new Point(0, optionsPanel.Height - 1), new Size(recordingRectangle.Width, recordingRectangle.Height - optionsPanel.Height + 2)).OffsetRectangle(1);
                excludeRectangle2 = new Rectangle(new Point(optionsPanel.Width + 1, 1), new Size(recordingRectangle.Width - optionsPanel.Width - 2, optionsPanel.Height));
                contentPadding = new Padding(1, 1, 0, 0);
            }

            excludeRectangles = new Rectangle[]
            {
                excludeRectangle,
                excludeRectangle2
            };
            Platforms.Windows.WindowsRegionHelper.Exclude(ControlObject, excludeRectangles);
#endif

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

        private void InitializeOptionsPanel()
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
