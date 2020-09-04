using System;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Date format item.
    /// </summary>
    internal class DateFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Date";

        /// <inheritdoc />
        public string Pattern => "date";

        /// <inheritdoc />
        public string GetText() => $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
    }
}
