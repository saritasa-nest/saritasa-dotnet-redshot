using Eto.Forms;
using RedShot.Infrastructure;
using RedShot.Shortcuts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Shortcut.Shortcuts
{
    internal class RecordShortcut : IShortcut
    {
        public string Name => "Record video";

        public Keys Keys { get; set; }

        public void OnPressedAction()
        {
            ApplicationManager.RunRecording();
        }
    }
}
