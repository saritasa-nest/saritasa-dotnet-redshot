using System.Collections.Generic;
using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Keys parser.
    /// </summary>
    public static class KeysParser
    {
        private static readonly IEnumerable<string> ValuesToRemove;
        private static readonly IDictionary<string, string> BracketReplaceValues;

        static KeysParser()
        {
            ValuesToRemove = new List<string>
            {
                "LeftAlt",
                "LeftShift",
                "LeftControl",
                "LeftApplication"
            };

            BracketReplaceValues = new Dictionary<string, string>
            {
                { "[", "]" },
                { "]", "[" },
            };
        }

        /// <summary>
        /// Get shortcut string.
        /// </summary>
        public static string GetShortcutString(Keys keys)
        {
            var shortcutString = keys.ToShortcutString();

            foreach (var value in ValuesToRemove)
            {
                shortcutString = shortcutString.Replace("+" + value, string.Empty);
                shortcutString = shortcutString.Replace(value, string.Empty);
            }

            foreach (var keyValue in BracketReplaceValues)
            {
                if (shortcutString.Contains(keyValue.Key))
                {
                    shortcutString = shortcutString.Replace(keyValue.Key, keyValue.Value);
                    break;
                }
            }

            return shortcutString;
        }
    }
}
