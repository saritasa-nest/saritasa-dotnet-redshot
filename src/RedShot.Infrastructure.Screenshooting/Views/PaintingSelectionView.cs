namespace RedShot.Infrastructure.Screenshooting.Views
{
    internal class PaintingSelectionView : ScreenShotSelectionView
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override string TopMessage { get; set; } = "Please select a region to paint";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void FinishSelection()
        {
            var screenshot = GetScreenShot();
            ScreenshotManager.RunPaintingView(screenshot);
            Close();
        }
    }
}
