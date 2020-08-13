using Eto.WinForms.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RedShot.Recording.Windows
{
    //public class WindowsRecordingControlHandler : WindowsControl<System.Windows.Forms.Control, RecordingControl, Eto.Forms.Control.ICallback>, IRecordingControl
    //{
    //    private bool disposed;

    //    /// <summary>
    //    /// Inits WIN OS SkiaSharp control.
    //    /// </summary>
    //    public WindowsRecordingControlHandler()
    //    {
    //        Control = new WindowsRecordingControl();
    //    }

    //    public Eto.Drawing.Rectangle CaptureArea { get; set; }

    //    public event EventHandler StartButtonClicked;

    //    public event EventHandler StopButtonClicked;

    //}

    //public class WindowsRecordingControl : Control
    //{
    //    private Button startButton;
    //    private Button stopButton;

    //    private Control managePanel;

    //    public WindowsRecordingControl()
    //    {
    //        Size = new Size(500, 500);

    //        startButton = new Button()
    //        {
    //            Text = "Start",
    //            Height = 50
    //        };

    //        stopButton = new Button()
    //        {
    //            Text = "Stop",
    //            Height = 50
    //        };

    //        managePanel = new FlowLayoutPanel()
    //        {
    //            Height = 50,
    //            Width = 500,
    //            Location = new Point(0, 0)
    //        };

    //        managePanel.Controls.Add(startButton);
    //        managePanel.Controls.Add(stopButton);

    //        Controls.Add(managePanel);

    //        var region = new Region(ClientRectangle);
    //        region.Exclude(new Rectangle(10, 100, 100, 100));
    //        Region = region;
    //    }
    //}
}