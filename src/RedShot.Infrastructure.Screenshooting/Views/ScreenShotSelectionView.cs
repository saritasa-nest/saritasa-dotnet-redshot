using RedShot.Infrastructure.Common.Forms.SelectionForm;

namespace RedShot.Infrastructure.Screenshooting.Views
{
    /// <summary>
    /// Screen shot selection view.
    /// </summary>
    internal class ScreenShotSelectionView : SelectionFormBase<ScreenShotPanel>
    {
        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Select the screenshot area";

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            ScreenshotManager.RunPaintingView(GetScreenShot());
            Close();
        }
    }
}
