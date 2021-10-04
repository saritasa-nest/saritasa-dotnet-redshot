using Eto.Drawing;
using RedShot.Eto.Desktop.Forms.Common;
using RedShot.Eto.Mvp.Presenters;
using RedShot.Infrastructure.Screenshooting.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Eto.Desktop.Forms
{
    internal class ScreenshotsForm : SelectionFormBase<ScreenShotPanel>, IScreenshotsView
    {
        public override event EventHandler<EventArgs> FormReadyToClose;

        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Select the screenshot area";

        public event EventHandler<Bitmap> ScreenshotUpdated;

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            var screenshot = GetScreenShot();
            ScreenshotUpdated?.Invoke(this, screenshot);
            FormReadyToClose?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
