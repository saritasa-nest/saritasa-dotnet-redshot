using Eto.Drawing;

namespace RedShot.Helpers
{
    /// <summary>
    /// Helps define styles for whole app.
    /// </summary>
    public static class StylesHelper
    {
        /// <summary>
        /// Background color.
        /// </summary>
        public static Color BackgroundColor { get; } = Color.FromArgb(42, 47, 56);

        /// <summary>
        /// Text color.
        /// </summary>
        public static Color TextColor { get; } = Colors.White;
    }
}
