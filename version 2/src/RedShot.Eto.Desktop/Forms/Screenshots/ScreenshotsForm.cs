using Eto.Drawing;
using RedShot.Eto.Desktop.Forms.Common;
using RedShot.Eto.Mvp.Presenters;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Eto.Desktop.Forms.Screenshots
{
    internal class ScreenshotsForm : SelectionFormBase, IScreenshotsView
    {
        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Select the screenshot area";

        public event EventHandler<Bitmap> ScreenshotUpdated;

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            var screenshot = GetScreenShot();
            ScreenshotUpdated?.Invoke(this, screenshot);
            CallReadyToClose();
        }
    }
}
