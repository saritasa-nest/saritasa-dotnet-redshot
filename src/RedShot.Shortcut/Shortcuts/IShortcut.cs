using Eto.Forms;

namespace RedShot.Shortcuts
{
    internal interface IShortcut
    {
        string Name { get; }

        Keys Keys { get; set; }

        void OnPressedAction();
    }
}
