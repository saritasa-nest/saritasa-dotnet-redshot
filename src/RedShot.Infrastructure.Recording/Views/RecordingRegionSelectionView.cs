using RedShot.Infrastructure.Common.Forms.SelectionForm;
using RedShot.Infrastructure.Recording;

namespace RedShot.Infrastructure.RecordingRedShot.Views
{
    /// <summary>
    /// Recording region selection view.
    /// </summary>
    internal sealed class RecordingRegionSelectionView : SelectionFormBase<RecordManagePanel>
    {
        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Please select a region to record";

        /// <inheritdoc/>
        protected override void InitializeSelectionManageForm()
        {
            base.InitializeSelectionManageForm();

            selectionManageForm.StartRecordingButton.Clicked += StartRecordingButton_Clicked;
        }

        private void StartRecordingButton_Clicked(object sender, System.EventArgs e)
        {
            FinishSelection();
        }

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            RecordingManager.RecordRegion(GetRealSelectionRegion());
            Close();
        }
    }
}
