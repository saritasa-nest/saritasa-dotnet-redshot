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
        public string Name => "Year";

        /// <inheritdoc />
        public string Pattern => "year";

        /// <inheritdoc />
        public string GetText() => DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
    }
}
