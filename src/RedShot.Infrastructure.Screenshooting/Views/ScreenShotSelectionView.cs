using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms.SelectionForm;

namespace RedShot.Infrastructure.Screenshooting.Views
{
    /// <summary>
    /// Screen shot selection view.
    /// </summary>
    internal class ScreenShotSelectionView : SelectionFormBase<ScreenShotPanel>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override string TopMessage { get; set; } = "Please select a region to capture";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void InitializeSelectionManageForm()
        {
            base.InitializeSelectionManageForm();
            selectionManageForm.FinishSelectionButton.Clicked += (o, e) => FinishSelection();
        }

        /// <summary>
        /// Get screen shot.
        /// </summary>
        protected Bitmap GetScreenShot()
        {
            return etoScreenImage.Clone(GetSelectionRegion());
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void FinishSelection()
        {
            var screenshot = GetScreenShot();
            ScreenshotManager.UploadScreenShot(screenshot);
            Close();
        }
    }
}
