namespace RedShot.Shortcuts
{
    internal interface IShortcut
    {
        string Name { get; }

        void OnPressedAction();
    }
}
