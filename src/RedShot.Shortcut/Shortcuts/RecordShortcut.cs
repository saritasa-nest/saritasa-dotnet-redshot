using Eto.Forms;
using RedShot.Infrastructure;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Record shortcut.
    /// </summary>
    internal class RecordShortcut : IShortcut
    {
        /// <inheritdoc/>
        public string Name => "Record video";

        /// <inheritdoc/>
        public Keys Keys { get; set; }

        /// <inheritdoc/>
        public void OnPressedAction()
        {
            ApplicationManager.RunRecording();
        }
    }
}
