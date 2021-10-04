using RedShot.Mvvm.ViewModels;
using System;

namespace RedShot.Desktop.Shared.Infrastructure.Navigation
{
    /// <summary>
    /// Adds a metadata to a view saying that it can be used with a specific viewmodel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class UsesViewModelAttribute : Attribute
    {
        /// <summary>
        /// Type of viewmodel this view should be used with.
        /// </summary>
        public Type ViewModelType { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UsesViewModelAttribute(Type viewModelType)
        {
            if (!viewModelType.IsSubclassOf(typeof(BaseViewModel)))
            {
                throw new ArgumentException($"Specified type {viewModelType.FullName} does not inherit {typeof(BaseViewModel).FullName}", nameof(viewModelType));
            }

            ViewModelType = viewModelType;
        }
    }
}
