namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Custom format
    /// </summary>
    internal class CustomFormatItem : IFormatItem
    {
        /// <inheritdoc/>
        public string Name => "Custom";

        /// <inheritdoc/>
        public string Pattern => "[your_text]";

        /// <inheritdoc/>
        public string GetText() => "your_text";
    }
}