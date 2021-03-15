using RedShot.Infrastructure.Recording;

namespace RedShot.Shortcut.Units
{
    /// <summary>
    /// Record shortcut.
    /// </summary>
    public sealed class RecordShortcut : BaseShortcut
    {
        /// <inheritdoc/>
        public override string Name => "Record video";

        /// <inheritdoc/>
        public override void OnPressedAction()
        {
            RecordingManager.Instance.InitiateRecording();
        }
    }
}
