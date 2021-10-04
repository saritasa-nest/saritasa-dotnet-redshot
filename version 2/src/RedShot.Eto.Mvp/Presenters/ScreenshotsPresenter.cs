using Eto.Drawing;
using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Eto.Mvp.Presenters.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.Presenters
{
    public interface IScreenshotsView : IView
    {
        event EventHandler<Bitmap> ScreenshotUpdated;
    }

    public class ScreenshotsPresenter : BasePresenter<IScreenshotsView>, IWithResult<byte[]>
    {
        public byte[] Result { get; private set; }

        public override async Task InitializePresenterAsync(IScreenshotsView view)
        {
            await base.InitializePresenterAsync(view);
            view.ScreenshotUpdated += ViewScreenshotUpdated;
        }

        private void ViewScreenshotUpdated(object sender, Bitmap screenshot)
        {
            Result = screenshot.ToByteArray(ImageFormat.Bitmap);
        }
    }
}
