using Eto.Drawing;
using RedShot.Helpers.Forms;
using System;

namespace RedShot.App.Screenshooting
{
    internal sealed class ScreenShotSelectionView : SelectionFormBase<ScreenShotPanel>
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
            ApplicationManager.RunPaintingView(screenshot);
        }

        private void SaveScreenShotButton_Clicked(object sender, EventArgs e)
        {
            FinishSelection();
        }

        protected override void FinishSelection()
        {
            if (captured)
            {
                ApplicationManager.RunUploadView(GetScreenShot());
                Close();
            }
        }
    }
}
