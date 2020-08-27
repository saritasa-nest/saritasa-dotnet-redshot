using Eto.Forms;

namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Abstraction for settings.
    /// </summary>
    public interface ISettingsOption
    {
        /// <summary>
        /// Setting name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gives option dialog.
        /// </summary>
        Dialog<DialogResult> GetOptionDialog();

        /// <summary>
        /// Saves setting state.
        /// </summary>
        void Save();
    }
}
