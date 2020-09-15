using RedShot.Infrastructure;
using RedShot.Shortcuts;

namespace RedShot.Shortcut.Shortcuts
{
    internal class RecordShortcut : IShortcut
    {
        public string Name => "Record video";

        public void OnPressedAction()
        {
            ApplicationManager.RunRecording();
        }
    }
}
