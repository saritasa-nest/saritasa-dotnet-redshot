using System;
using System.Collections.Generic;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Arguments helper.
    /// </summary>
    public static class ArgumentsHelper
    {
        /// <summary>
        /// Split lines.
        /// </summary>
        public static IEnumerable<string> SplitLines(string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }
    }
}
