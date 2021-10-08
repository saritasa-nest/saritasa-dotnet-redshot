using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IClipboardService
    {
        void SaveToClipboard(string text);
    }
}
