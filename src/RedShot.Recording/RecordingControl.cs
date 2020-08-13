using System;
using Eto;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Recording
{
    [Handler(typeof(IRecordingControl))]
    public class RecordingControl : Control
    {
        new IRecordingControl Handler { get { return (IRecordingControl)base.Handler; } }

        public Rectangle CaptureArea
        {
            get
            {
                return Handler.CaptureArea;
            }

            set
            {
                Handler.CaptureArea = value;
            }
        }

        public event EventHandler StartButtonClicked
        {
            add
            {
                Handler.StartButtonClicked += value;
            }

            remove
            {
                Handler.StartButtonClicked -= value;
            }
        }

        public event EventHandler StopButtonClicked
        {
            add
            {
                Handler.StopButtonClicked += value;
            }

            remove
            {
                Handler.StopButtonClicked -= value;
            }
        }

        // interface to the platform implementations
        public interface IRecordingControl : Control.IHandler
        {
            Rectangle CaptureArea { get; set; }

            event EventHandler StartButtonClicked;

            event EventHandler StopButtonClicked;
        }
    }
}
