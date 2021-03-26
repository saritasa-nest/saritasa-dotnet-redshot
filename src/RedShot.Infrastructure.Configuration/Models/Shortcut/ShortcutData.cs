using Eto.Forms;

namespace RedShot.Infrastructure.Configuration.Models.Shortcut
{
    /// <summary>
    /// Contains data about shortcut parameters.
    /// </summary>
    public class ShortcutData
    {
        /// <summary>
        /// Shortcut name.
        /// </summary>
        public string ShortcutName { get; set; }

        /// <summary>
        /// Hot keys.
        /// </summary>
        public Keys Keys { get; set; }
    }
}
