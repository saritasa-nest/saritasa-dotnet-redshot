using RedShot.Helpers.Forms;

namespace RedShot.Infrastructure.RecordingRedShot.Views
{
    internal sealed class RecordingRegionSelectionView : SelectionFormBase<RecordManagePanel>
    {
        protected override void InitializeSelectionManageForm()
        {
            base.InitializeSelectionManageForm();

            selectionManageForm.StartRecordingButton.Clicked += StartRecordingButton_Clicked;
        }

        private void StartRecordingButton_Clicked(object sender, System.EventArgs e)
        {
            FinishSelection();
        }

        protected override void FinishSelection()
        {
            RecordingManager.RecordRegion(GetSelectionRegion());
            Close();
        }
    }
}
