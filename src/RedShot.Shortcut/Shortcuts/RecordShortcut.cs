using RedShot.Infrastructure;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Record shortcut.
    /// </summary>
    internal sealed class RecordShortcut : Shortcut
    {
        /// <inheritdoc/>
        public override string Name => "Record video";

        /// <inheritdoc/>
        public override void OnPressedAction()
        {
            ApplicationManager.RunRecording();
        }
    }
}
