using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Formatting.Formatters;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Format service.
    /// </summary>
    public static class FormatManager
    {
        /// <summary>
        /// Format Tag.
        /// </summary>
        public static string FormatTag { get; }

        private static readonly string regexPattern = @$"{FormatTag}(\w+)|{FormatTag}\[(\w|-|_)+\]";
        private static readonly string usersTextPattern = @"^\[(\w|-|_)+\]$";
        private static readonly RegexOptions regexOptions = RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;
        private static readonly Regex formatExpression = new Regex(regexPattern, regexOptions);

        static FormatManager()
        {
            FormatTag = ConfigurationManager.AppSettings.FormatFileNameTag;

            FormatItems = Assembly
                .GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IFormatItem).IsAssignableFrom(type) && !type.IsInterface)
                .Select(t => (IFormatItem) Activator.CreateInstance(t));
        }

        internal static readonly IEnumerable<IFormatItem> FormatItems;

        /// <summary>
        /// Try to format pattern.
        /// </summary>
        internal static bool TryFormat(string pattern, out string formattedString)
        {
            formattedString = Format(pattern);

            return !string.IsNullOrWhiteSpace(formattedString);
        }

        /// <summary>
        /// Get format name of for a file.
        /// Gets pattern from configuration.
        /// </summary>
        public static string GetFormattedName()
        {
            var pattern = GetPatternFromConfig();
            var formattedString = Format(pattern);

            if (string.IsNullOrEmpty(formattedString) || string.IsNullOrWhiteSpace(formattedString))
            {
                return "RedShot_invalid_format";
            }
            else
            {
                return formattedString;
            }
        }

        private static string Format(string pattern)
        {
            var matches = formatExpression.Matches(pattern);
            var builder = new StringBuilder();

            foreach (var match in matches)
            {
                var formatMatch = match.ToString().Replace("%", "");
                if (FormatItems.FirstOrDefault(f => f.Pattern.Equals(formatMatch, StringComparison.InvariantCultureIgnoreCase)) is IFormatItem formatItem)
                {
                    builder.Append(formatItem.GetText());
                }
                else
                {
                    if (TryFormatAsUsersText(formatMatch, out var text))
                    {
                        builder.Append(text);
                    }
                }
            }

            var formattedString = builder.ToString();
            ReplaceInvalidChars(ref formattedString);

            return formattedString;
        }

        private static bool TryFormatAsUsersText(string text, out string result)
        {
            var regex = new Regex(usersTextPattern, regexOptions);

            if (regex.IsMatch(text))
            {
                var matched = regex.Match(text).ToString();

                result = matched.Replace("[", "").Replace("]", "");
                return true;
            }

            result = string.Empty;
            return false;
        }

        private static void ReplaceInvalidChars(ref string formattedString)
        {
            foreach (var invalidPathChar in Path.GetInvalidPathChars())
            {
                formattedString = formattedString.Replace(invalidPathChar.ToString(), "");
            }
        }

        private static string GetPatternFromConfig()
        {
            return ConfigurationManager.GetSection<GeneralConfigurationOption>().Pattern;
        }
    }
}