using System;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Guid format item.
    /// </summary>
    internal class GuidFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Guid";

        /// <inheritdoc />
        public string Pattern => "guid";

        /// <inheritdoc />
        public string GetText() => Guid.NewGuid().ToString();
    }
}
