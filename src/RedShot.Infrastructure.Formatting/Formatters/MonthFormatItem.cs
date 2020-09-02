using System;
using System.Globalization;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Month format item.
    /// </summary>
    internal class MonthFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Month";

        /// <inheritdoc />
        public string Pattern => "month";

        /// <inheritdoc />
        public string GetText()
        {
            return DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
        }
    }
}