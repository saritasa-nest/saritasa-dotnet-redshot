using System;
using Eto.Forms;

namespace RedShot.Infrastructure.Abstractions.Settings
{
    /// <summary>
    /// Settings section.
    /// </summary>
    public interface ISettingsSection : IDisposable
    {
        /// <summary>
        /// Setting name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get setting control.
        /// </summary>
        Control GetControl();

        /// <summary>
        /// Save settings state.
        /// </summary>
        void Save();
    }
}
