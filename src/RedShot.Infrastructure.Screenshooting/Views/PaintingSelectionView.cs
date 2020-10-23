using RedShot.Infrastructure.Common.Forms.SelectionForm;

namespace RedShot.Infrastructure.Screenshooting.Views
{
    /// <summary>
    /// Painting selection view.
    /// </summary>
    internal class PaintingSelectionView : SelectionFormBase<PaintingPanel>
    {
        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Please select a region to paint";

        /// <inheritdoc/>
        protected override void InitializeSelectionManageForm()
        {
            base.InitializeSelectionManageForm();
            selectionManageForm.FinishSelectionButton.Clicked += (o, e) => FinishSelection();
        }

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            ScreenshotManager.RunPaintingView(GetScreenShot());
            Close();
        }
    }
}
