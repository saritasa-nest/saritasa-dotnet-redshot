using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Eto.Mvp.Presenters.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.Presenters.Records
{
    public interface IRecordAreaSelectionView : IView
    {
        event EventHandler<Rectangle> SelectionUpdated;

        event EventHandler ScreenChanged;
    }

    public enum SelectionStatus
    {
        Canceled,
        NeedToChangeScreen,
        Selected
    }

    public struct SelectionResult
    {
        public SelectionStatus Status { get; set; }

        public Rectangle SelectionArea { get; set; }
    }

    public class RecordAreaSelectionPresenter : BasePresenter<IRecordAreaSelectionView>, IWithResult<SelectionResult>
    {
        public SelectionResult Result { get; private set; }

        public override async Task InitializePresenterAsync(IRecordAreaSelectionView view)
        {
            await base.InitializePresenterAsync(view);
            view.SelectionUpdated += ViewSelectionUpdated;
            view.ScreenChanged += ViewScreenChanged;
            view.FormReadyToClose += ViewFormReadyToClose;
        }

        private void ViewSelectionUpdated(object sender, Rectangle area)
        {
            Result = new SelectionResult()
            {
                SelectionArea = area,
                Status = SelectionStatus.Selected
            };
        }

        private void ViewFormReadyToClose(object sender, EventArgs e)
        {
            View.Close();
        }

        private void ViewScreenChanged(object sender, EventArgs e)
        {
            Result = new SelectionResult()
            {
                Status = SelectionStatus.NeedToChangeScreen
            };
        }
    }
}
