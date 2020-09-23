using System.Windows.Forms;

namespace RedShot.Platforms.Windows
{
    /// <summary>
    /// Windows native helper.
    /// </summary>
    public static class WindowsNativeHelper
    {
        /// <summary>
        /// Hide caret.
        /// </summary>
        public static void HideCaret(object windowsControl)
        {
            if (windowsControl is Control control)
            {
                control.GotFocus += (sender, e) =>
                {
                    NativeMethods.HideCaret(control.Handle);
                };
            }
        }
    }
}
