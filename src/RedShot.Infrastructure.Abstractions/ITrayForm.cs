using Eto.Forms;

namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Tray form.
    /// </summary>
    public interface ITrayForm
    {
        /// <summary>
        /// Tray indicator.
        /// </summary>
        TrayIndicator Tray { get; }
    }
}
