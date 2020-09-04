using Eto.Forms;

namespace RedShot.Infrastructure.Settings.Sections
{
    /// <summary>
    /// Abstraction for settings.
    /// </summary>
    public interface ISettingsSection
    {
        /// <summary>
        /// Setting name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gives option dialog.
        /// </summary>
        Control GetControl();

        /// <summary>
        /// Saves setting state.
        /// </summary>
        void Save();
    }
}
