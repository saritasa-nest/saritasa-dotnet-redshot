using System;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms.SelectionForm;

namespace RedShot.Infrastructure.Screenshooting.Views
{
    /// <summary>
    /// Screen shot selection view.
    /// </summary>
    internal class ScreenShotSelectionView : SelectionFormBase<ScreenShotPanel>
    {
        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Please select a region to capture";

        /// <inheritdoc/>
        protected override void InitializeSelectionManageForm()
        {
            base.InitializeSelectionManageForm();
            selectionManageForm.FinishSelectionButton.Clicked += (o, e) => FinishSelection();
        }

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            var screenshot = GetScreenShot();
            selectionManageForm.Minimize();
            Minimize();
            Close();
            ScreenshotManager.UploadScreenShot(screenshot);
        }
    }
}
