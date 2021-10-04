using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Eto.Mvp.Presenters.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.ServiceAbstractions
{
    public interface IEtoNavigationService
    {
        Task OpenFormAsync<P, V>(params object[] parameters)
            where P : BasePresenter<V>
            where V : IView;

        Task OpenFormAsync<P, V>(P presenter)
            where P : BasePresenter<V>
            where V : IView;

        Task<Result> OpenFormAsync<P, V, Result>(params object[] parameters)
            where P : BasePresenter<V>, IWithResult<Result>
            where V : IView;

        Task<Result> OpenFormAsync<P, V, Result>(P presenter)
            where P : BasePresenter<V>, IWithResult<Result>
            where V : IView;
    }
}
