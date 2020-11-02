using System.Collections.Generic;
using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Keys parser.
    /// </summary>
    public class KeysParser
    {
        /// <summary>
        /// Get shortcut string.
        /// </summary>
        public string GetShortcutString(Keys keys)
        {
            var shortcutString = keys.ToShortcutString();

            shortcutString = FixValues(shortcutString);
            shortcutString = FixBrackets(shortcutString);

            return shortcutString;
        }

        private string FixBrackets(string shortcutString)
        {
            var bracketReplaceValues = new Dictionary<string, string>
            {
                { "[", "]" },
                { "]", "[" },
            };

            foreach (var keyValue in bracketReplaceValues)
            {
                if (shortcutString.Contains(keyValue.Key))
                {
                    shortcutString = shortcutString.Replace(keyValue.Key, keyValue.Value);
                    break;
                }
            }

            return shortcutString;
        }

        private string FixValues(string shortcutString)
        {
            var valuesToRemove = new List<string>
            {
                "LeftAlt",
                "LeftShift",
                "LeftControl",
                "LeftApplication"
            };

            foreach (var value in valuesToRemove)
            {
                shortcutString = shortcutString.Replace("+" + value, string.Empty);
                shortcutString = shortcutString.Replace(value, string.Empty);
            }

            return shortcutString;
        }
    }
}
