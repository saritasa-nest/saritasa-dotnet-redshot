using Eto.Drawing;
using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Eto.Mvp.Presenters.Common;
using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.Presenters
{
    public interface IScreenshotsView : IView
    {
        event EventHandler<Bitmap> ScreenshotUpdated;

        event EventHandler ScreenChanged;
    }

    public enum ScreenshotStatus
    {
        Canceled,
        NeedToChangeScreen,
        Captured
    }

    public struct ScreenshotResult
    {
        public ScreenshotStatus Status { get; set; }

        public byte[] Screenshot { get; set; }
    }

    public class ScreenshotsPresenter : BasePresenter<IScreenshotsView>, IWithResult<ScreenshotResult>
    {
        public ScreenshotResult Result { get; private set; }

        public override async Task InitializePresenterAsync(IScreenshotsView view)
        {
            await base.InitializePresenterAsync(view);
            view.ScreenshotUpdated += ViewScreenshotUpdated;
            view.ScreenChanged += ViewScreenChanged;
            view.FormReadyToClose += ViewFormReadyToClose;
        }

        private void ViewFormReadyToClose(object sender, EventArgs e)
        {
            View.Close();
        }

        private void ViewScreenChanged(object sender, EventArgs e)
        {
            Result = new ScreenshotResult()
            {
                Status = ScreenshotStatus.NeedToChangeScreen
            };
        }

        private void ViewScreenshotUpdated(object sender, Bitmap screenshot)
        {
            var screenshotBytes = screenshot.ToByteArray(ImageFormat.Bitmap);
            Result = new ScreenshotResult()
            {
                Screenshot = screenshotBytes,
                Status = ScreenshotStatus.Captured
            };
        }
    }
}
