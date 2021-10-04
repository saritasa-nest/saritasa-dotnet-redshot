using Eto.Forms;
using Microsoft.Extensions.DependencyInjection;
using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Eto.Desktop.Forms.Common;
using RedShot.Eto.Mvp.Presenters.Common;
using RedShot.Eto.Mvp.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Eto.Desktop.Infrastructure
{
    internal class EtoNavigationService : IEtoNavigationService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IReadOnlyCollection<Type> formsTypes;

        public EtoNavigationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            formsTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(NavigationForm)))
                .Where(t => typeof(IView).IsAssignableFrom(t))
                .ToList();
        }

        public async Task OpenFormAsync<P, V>(params object[] parameters)
            where P : BasePresenter<V>
            where V : IView
        {
            var presenter = CreatePresenter<P, V>(parameters);
            await OpenInternal(presenter);
        }

        public async Task OpenFormAsync<P, V>(P presenter)
            where P : BasePresenter<V>
            where V : IView
        {
            await OpenInternal(presenter);
        }

        public async Task<Result> OpenFormAsync<P, V, Result>(params object[] parameters)
            where P : BasePresenter<V>, IWithResult<Result>
            where V : IView
        {
            var presenter = CreatePresenter<P, V>(parameters);
            await OpenInternal(presenter);
            return (presenter as IWithResult<Result>).Result;
        }

        public async Task<Result> OpenFormAsync<P, V, Result>(P presenter)
            where P : BasePresenter<V>, IWithResult<Result>
            where V : IView
        {
            await OpenInternal(presenter);
            return presenter.Result;
        }

        private async Task OpenInternal<T>(BasePresenter<T> presenter)
            where T : IView
        {
            var form = GetForm<T>();
            var application = await Startup.ApplicationInitializedCompletionSource.Task;
            await presenter.InitializePresenterAsync(form);
            var task = await application.InvokeAsync(() =>
            {
                var cancellationSource = new TaskCompletionSource<object>();

                var etoForm = form as NavigationForm;
                etoForm.FormReadyToClose += (o, e) => cancellationSource?.TrySetResult(default);
                etoForm.FormReadyToCancel += (o, e) => cancellationSource?.TrySetCanceled(default);
                etoForm.Show();

                return cancellationSource.Task;
            });
            await task;
        }

        private T GetForm<T>()
            where T : IView
        {
            var formType = formsTypes.SingleOrDefault(t => typeof(T).IsAssignableFrom(t));
            if (formType == null)
            {
                throw new ArgumentException($"Cannot find a form type for the {typeof(T)} view.");
            }

            return (T)Activator.CreateInstance(formType);
        }

        private BasePresenter<V> CreatePresenter<P, V>(params object[] parameters)
            where P : BasePresenter<V>
            where V : IView
        {
            return ActivatorUtilities.CreateInstance<P>(serviceProvider, parameters);
        }
    }
}
