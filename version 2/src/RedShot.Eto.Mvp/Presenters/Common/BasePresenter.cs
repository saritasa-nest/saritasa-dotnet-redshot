using Eto.Forms;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.Presenters.Common
{
    /// <summary>
    /// View abstraction for presenter.
    /// </summary>
    public interface IView { };

    /// <summary>
    /// Base presenter class.
    /// </summary>
    /// <typeparam name="T">View abstraction type.</typeparam>
    public abstract class BasePresenter<T>
        where T : IView
    {
        /// <summary>
        /// Initialize presenter.
        /// </summary>
        /// <param name="view">View abstraction.</param>
        public virtual Task InitializePresenterAsync(T view)
        {
            return Task.CompletedTask;
        }
    }
}
