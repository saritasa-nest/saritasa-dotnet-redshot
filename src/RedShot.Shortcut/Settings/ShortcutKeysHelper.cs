using System.Collections.Generic;
using System.Text;
using Eto;
using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Keys parser.
    /// </summary>
    internal class ShortcutKeysHelper
    {
        private static readonly List<Keys> notToUseKeys = new List<Keys>
        {
            Keys.LeftShift,
            Keys.RightShift,
            Keys.LeftControl,
            Keys.RightControl,
            Keys.LeftAlt,
            Keys.RightAlt,
            Keys.LeftApplication,
            Keys.RightApplication
        };

        private static readonly Dictionary<Keys, string> keymap = new Dictionary<Keys, string>
        {
            { Keys.D0, "0" },
            { Keys.D1, "1" },
            { Keys.D2, "2" },
            { Keys.D3, "3" },
            { Keys.D4, "4" },
            { Keys.D5, "5" },
            { Keys.D6, "6" },
            { Keys.D7, "7" },
            { Keys.D8, "8" },
            { Keys.D9, "9" },
            { Keys.Minus, "-" },
            { Keys.Equal, "=" },
            { Keys.Grave, "`" },
            { Keys.Divide, "/" },
            { Keys.Decimal, "." },
            { Keys.Backslash, "\\" },
            { Keys.KeypadEqual, "=" },
            { Keys.Multiply, "*" },
            { Keys.Add, "+" },
            { Keys.Subtract, "-" },
            { Keys.Tab, "\x21E5" },
            { Keys.Enter, "\x23ce" },
            { Keys.Delete, EtoEnvironment.Platform.IsMac ? "\x232b" : "Del" },
            { Keys.Escape, EtoEnvironment.Platform.IsMac ? "\x238b" : "Esc" },
            { Keys.Semicolon, ";" },
            { Keys.Quote, "'" },
            { Keys.Comma, "," },
            { Keys.Period, "." },
            { Keys.Slash, "/" },
            { Keys.RightBracket, "]" },
            { Keys.LeftBracket, "[" }
        };

        /// <summary>
        /// Converts the specified key to a shortcut string such as Ctrl+Alt+Z.
        /// </summary>
        /// <param name="keys">Keys to convert.</param>
        /// <param name="separator">Separator between each modifier and key.</param>
        /// <returns>A human-readable string representing the key combination including modifiers.</returns>
        public string GetShortcutString(Keys keys, string separator = "+")
        {
            var sb = new StringBuilder();

            if (keys.HasFlag(Keys.Application))
            {
                AppendSeparator(sb, separator,
                    EtoEnvironment.Platform.IsMac ? "\x2318" :
                    EtoEnvironment.Platform.IsWindows ? "Win" :
                    "App");
            }
            if (keys.HasFlag(Keys.Control))
            {
                AppendSeparator(sb, separator, EtoEnvironment.Platform.IsMac ? "^" : "Ctrl");
            }
            if (keys.HasFlag(Keys.Shift))
            {
                AppendSeparator(sb, separator, EtoEnvironment.Platform.IsMac ? "\x21e7" : "Shift");
            }
            if (keys.HasFlag(Keys.Alt))
            {
                AppendSeparator(sb, separator, EtoEnvironment.Platform.IsMac ? "\x2325" : "Alt");
            }

            if (TryGetMainKey(keys, out Keys mainKey))
            {
                if (keymap.TryGetValue(mainKey, out string value))
                {
                    AppendSeparator(sb, separator, value);
                }
                else
                {
                    AppendSeparator(sb, separator, mainKey.ToString());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Try to get the main key.
        /// </summary>
        public bool TryGetMainKey(Keys keyData, out Keys mainKey)
        {
            mainKey = keyData & Keys.KeyMask;

            return !notToUseKeys.Contains(mainKey) && mainKey != Keys.None;
        }

        private void AppendSeparator(StringBuilder sb, string separator, string value)
        {
            if (sb.Length > 0)
            {
                sb.Append(separator);
            }
            sb.Append(value);
        }
    }
}
