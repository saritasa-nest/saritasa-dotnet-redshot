using System;
using System.Globalization;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Year format item.
    /// </summary>
    internal class YearFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Current year";

        /// <inheritdoc />
        public string Pattern => "year";

        /// <inheritdoc />
        public string GetText()
        {
            return DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
        }
    }
}