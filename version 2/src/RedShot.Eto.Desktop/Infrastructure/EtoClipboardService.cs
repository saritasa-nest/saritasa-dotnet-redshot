using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Eto.Desktop.Infrastructure
{
    internal class EtoClipboardService : IClipboardService
    {
        public void SaveToClipboard(string text)
        {
            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = text;
        }
    }
}
