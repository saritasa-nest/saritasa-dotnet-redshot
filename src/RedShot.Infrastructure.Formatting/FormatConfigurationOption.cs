using RedShot.Infrastructure.Abstractions;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Format configuration option.
    /// </summary>
    public class FormatConfigurationOption : IConfigurationOption
    {
        public FormatConfigurationOption()
        {
            Pattern = "%[RedShot-]%date";
        }

        /// <inheritdoc />
        public string UniqueName => "Formatting";

        /// <summary>
        /// Pattern for formatting.
        /// </summary>
        public string Pattern { get; set; }

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}