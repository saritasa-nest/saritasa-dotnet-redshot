using System;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Desktop.Shared.Infrastructure.Navigation
{
    /// <summary>
    /// Contains stack of all active views in the application.
    /// </summary>
    public class NavigationStack
    {
        private readonly List<ViewStackItem> viewItems = new ();

        /// <summary>
        /// Get the top view from the stack and remove it.
        /// </summary>
        /// <typeparam name="T">View type / owner type.</typeparam>
        /// <returns>Top view or <c>null</c> if there are no views with specified owner type.</returns>
        public ViewState Pop<T>()
        {
            var topItemIndex = GetFirstStackItemIndex<T>();
            if (topItemIndex == null)
            {
                return null;
            }
            // In our application this should never happen because we can only close the topmost view
            // If this is happening, it is likely a bug?
            if (topItemIndex != 0)
            {
                throw new ArgumentException("Specified owner type does not match the topmost view owner type.");
            }

            var viewState = viewItems[topItemIndex.Value];
            viewItems.RemoveAt(topItemIndex.Value);
            return viewState.View;
        }

        /// <summary>
        /// Get the top view from the stack.
        /// </summary>
        /// <typeparam name="T">View type / owner type.</typeparam>
        /// <returns>Top view or <c>null</c> if there are no views with specified owner type.</returns>
        public ViewState Peek<T>()
        {
            var topMatchingItemIndex = GetFirstStackItemIndex<T>();
            if (topMatchingItemIndex == null)
            {
                return null;
            }

            var viewState = viewItems[topMatchingItemIndex.Value];
            return viewState.View;
        }

        /// <summary>
        /// Get the top view from the stack.
        /// </summary>
        /// <returns>Top view reference.</returns>
        public ViewState Peek()
        {
            return viewItems.FirstOrDefault()?.View;
        }

        /// <summary>
        /// Add a new view to the stack.
        /// </summary>
        /// <typeparam name="T">View type / owner type.</typeparam>
        /// <param name="view">View instance.</param>
        public void Push<T>(ViewState view)
        {
            var state = new ViewStackItem()
            {
                View = view,
                OwnerType = typeof(T)
            };
            viewItems.Insert(0, state);
        }

        private int? GetFirstStackItemIndex<T>()
        {
            var ownerType = typeof(T);
            for (var i = 0; i < viewItems.Count; ++i)
            {
                if (viewItems[i].OwnerType == ownerType)
                {
                    return i;
                }
            }

            return null;
        }

        private class ViewStackItem
        {
            public ViewState View { get; set; }

            public Type OwnerType { get; set; }
        }
    }
}
