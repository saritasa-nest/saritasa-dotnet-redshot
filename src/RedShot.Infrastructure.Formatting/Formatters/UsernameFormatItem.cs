using System;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Username format item.
    /// </summary>
    internal class UsernameFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Username";

        /// <inheritdoc />
        public string Pattern => "user";

        /// <inheritdoc />
        public string GetText() =>Environment.UserName;
    }
}
