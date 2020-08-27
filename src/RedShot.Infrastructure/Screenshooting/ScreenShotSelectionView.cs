using System;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms.SelectionForm;

namespace RedShot.Infrastructure.Screenshooting
{
    /// <summary>
    /// Screenshot selection view.
    /// </summary>
    public sealed class ScreenShotSelectionView : SelectionFormBase<ScreenShotPanel>
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

            selectionManageForm.EnablePaintingModeButton.Clicked += EnablePaintingModeButton_Clicked;
            selectionManageForm.SaveScreenShotButton.Clicked += SaveScreenShotButton_Clicked;
        }

        private Bitmap GetScreenShot()
        {
            return etoScreenImage.Clone(GetSelectionRegion());
        }

        private void EnablePaintingModeButton_Clicked(object sender, EventArgs e)
        {
            var screenshot = GetScreenShot();
            ScreenshotManager.RunPaintingView(screenshot);
            Close();
        }

        private void SaveScreenShotButton_Clicked(object sender, EventArgs e)
        {
            FinishSelection();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void FinishSelection()
        {
            if (captured)
            {
                ScreenshotManager.UploadScreenShot(GetScreenShot());
                Close();
            }
        }
    }
}
