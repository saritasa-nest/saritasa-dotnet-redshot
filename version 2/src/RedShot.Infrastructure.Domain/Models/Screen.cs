namespace RedShot.Infrastructure.Domain.Models
{
    /// <summary>
    /// Screen model.
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Screen name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Preview Image.
        /// </summary>
        public byte[] PreviouImage { get; init; }
    }
}
