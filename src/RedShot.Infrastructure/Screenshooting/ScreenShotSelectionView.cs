﻿using Eto.Drawing;
using RedShot.Infrastructure.Common.Forms.SelectionForm;
using System;

namespace RedShot.Infrastructure.Screenshooting
{
    public sealed class ScreenShotSelectionView : SelectionFormBase<ScreenShotPanel>
    {
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
