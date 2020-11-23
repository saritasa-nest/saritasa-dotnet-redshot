using Eto.Forms;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.TextActions
{
    /// <summary>
    /// Text dialog result.
    /// </summary>
    internal class TextDialogResult
    {
        /// <summary>
        /// Dialog result.
        /// </summary>
        public DialogResult DialogResult { get; set; }

        /// <summary>
        /// Text input action.
        /// </summary>
        public TextInputAction TextInputAction { get; set; }
    }
}
