using Eto.Forms;
using System;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.Presenters.Common
{
    /// <summary>
    /// View abstraction for presenter.
    /// </summary>
    public interface IView
    {
        event EventHandler<EventArgs> FormReadyToClose;

        void Close();
    }

    /// <summary>
    /// Base presenter class.
    /// </summary>
    /// <typeparam name="T">View abstraction type.</typeparam>
    public abstract class BasePresenter<T>
        where T : IView
    {
        protected T View { get; private set; }

        /// <summary>
        /// Initialize presenter.
        /// </summary>
        /// <param name="view">View abstraction.</param>
        public virtual Task InitializePresenterAsync(T view)
        {
            View = view;
            return Task.CompletedTask;
        }
    }
}
