using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Abstractions.Settings
{
    /// <summary>
    /// Validatable section.
    /// </summary>
    public interface IValidatableSection : ISettingsSection
    {
        /// <summary>
        /// Validate the settings section.
        /// </summary>
        ValidationResult Validate();
    }
}
