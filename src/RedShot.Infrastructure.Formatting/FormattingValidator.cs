using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Formatting validator.
    /// </summary>
    internal static class FormattingValidator
    {
        /// <summary>
        /// Validate pattern.
        /// </summary>
        public static ValidationResult Validate(string pattern)
        {
            if (FormatManager.TryFormat(pattern, out _))
            {
                return new ValidationResult(true);
            }
            else
            {
                return new ValidationResult(false, "Invalid pattern in format link");
            }
        }
    }
}
