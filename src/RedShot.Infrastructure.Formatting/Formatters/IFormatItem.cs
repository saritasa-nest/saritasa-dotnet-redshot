namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Format item.
    /// </summary>
    public interface IFormatItem
    {
        /// <summary>
        /// Name of the format item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Pattern of the format item.
        /// </summary>
        string Pattern { get; }

        /// <summary>
        /// Get text of the format item.
        /// </summary>
        string GetText();
    }
}