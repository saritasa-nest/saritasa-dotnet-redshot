namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Custom format item.
    /// </summary>
    public class CustomFormatItem : IFormatItem
    {
        /// <inheritdoc/>
        public string Name => "Custom";

        /// <inheritdoc/>
        public string Pattern => "[your_text]";

        /// <inheritdoc/>
        public string GetText() => "your_text";
    }
}