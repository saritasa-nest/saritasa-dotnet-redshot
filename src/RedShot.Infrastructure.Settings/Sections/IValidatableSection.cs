namespace RedShot.Infrastructure.Settings.Sections
{
    /// <summary>
    /// Validatable section.
    /// </summary>
    public interface IValidatableSection : ISettingsSection
    {
        /// <summary>
        /// Validate the settings section.
        /// </summary>
        Common.ValidationResult Validate();
    }
}
