using RedShot.Eto.Desktop.Forms.Common;
using RedShot.Eto.Mvp.Presenters.Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RedShot.Eto.Desktop.Forms.Records
{
    public class RecordAreaSelectionForm : SelectionFormBase, IRecordAreaSelectionView
    {
        /// <inheritdoc/>
        protected override string TopMessage { get; set; } = "Please select a region to record";

        public event EventHandler<Rectangle> SelectionUpdated;

        /// <inheritdoc/>
        protected override void FinishSelection()
        {
            var selectionArea = GetRealSelectionRegion();
            var systemFormatArea = new System.Drawing.Rectangle(selectionArea.X, selectionArea.Y, selectionArea.Width, selectionArea.Height);
            SelectionUpdated?.Invoke(this, systemFormatArea);
            CallReadyToClose();
        }
    }
}
