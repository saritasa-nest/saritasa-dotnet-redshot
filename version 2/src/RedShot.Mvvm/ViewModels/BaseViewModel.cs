using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace RedShot.Mvvm.ViewModels
{
    /// <summary>
    /// Base class for view models.
    /// </summary>
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        /// <summary>
        /// Is a view model busy.
        /// </summary>
        public bool IsBusy { get; set; }

        /// <summary>
        /// Loading data.
        /// </summary>
        public virtual Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        private bool disposedValue;

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
