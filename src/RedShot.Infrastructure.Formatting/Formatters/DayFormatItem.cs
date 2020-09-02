using System;
using System.Globalization;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Day of the week format item.
    /// </summary>
    internal class DayFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Day of the week";

        /// <inheritdoc />
        public string Pattern => "day";

        /// <inheritdoc />
        public string GetText()
        {
            return DateTime.Now.ToString("dddd", CultureInfo.InvariantCulture);
        }
    }
}