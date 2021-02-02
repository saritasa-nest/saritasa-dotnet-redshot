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
            optionsPanelSize = new Size(220, 41);

            recordingTimer = new Stopwatch();
            renderingTimer = new UITimer
            {
                Interval = 0.01
            };
            renderingTimer.Elapsed += RenderingTimerElapsed;
            renderingTimer.Start();

            var recordingVideoPanel = GetOptionsPanel();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    recordingVideoPanel
                }
            };

#if _WINDOWS
            WindowStyle = WindowStyle.None;
            BackgroundColor = Colors.Red;

            Rectangle excludeRectangle = default;
            Rectangle excludeRectangle2 = default;

            if ((recordingRectangle.Location.Y - recordingVideoPanel.Height) > 0)
            {
                Location = new Point(recordingRectangle.X, recordingRectangle.Y - recordingVideoPanel.Height - 1);
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + recordingVideoPanel.Height + 1);
                excludeRectangle = new Rectangle(new Point(0, recordingVideoPanel.Height), new Size(recordingRectangle.Width, recordingRectangle.Height + 1)).OffsetRectangle(1);
                excludeRectangle2 = new Rectangle(new Point(recordingVideoPanel.Width, 0), new Size(recordingRectangle.Width - recordingVideoPanel.Width, recordingVideoPanel.Height));
            }
            else
            {
                var screenBounds = ScreenHelper.GetScreenBounds();

                if ((recordingRectangle.Location.Y + recordingVideoPanel.Height) < screenBounds.Height)
                {
                    Location = recordingRectangle.Location;
                    Size = new Size(recordingRectangle.Width, recordingRectangle.Height + 1);
                    excludeRectangle = new Rectangle(new Point(0, recordingVideoPanel.Height - 1), new Size(recordingRectangle.Width, recordingRectangle.Height - recordingVideoPanel.Height + 2)).OffsetRectangle(1);
                    excludeRectangle2 = new Rectangle(new Point(recordingVideoPanel.Width, 1), new Size(recordingRectangle.Width - recordingVideoPanel.Width - 1, recordingVideoPanel.Height));
                }
            }

            RedShot.Platforms.Windows.WindowsRegionHelper.Exclude(this.ControlObject, excludeRectangle, excludeRectangle2);
#endif
        }

        private Control GetOptionsPanel()
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
            closeButton.Clicked += CloseButtonClicked;

            optionsButton = new ImageButton(buttonSize, Icons.Gear, scaleImageSize: scaleSize);
            optionsButton.ToolTip = "Audio Options";
            optionsButton.Clicked += OptionsButtonClicked;

            return new StackLayout()
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
