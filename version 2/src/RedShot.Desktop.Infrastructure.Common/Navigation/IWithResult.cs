namespace RedShot.Desktop.Infrastructure.Common.Navigation
{
    /// <summary>
    /// Indicates that associated entity can produce a result.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    public interface IWithResult<out T>
    {
        /// <summary>
        /// Result data.
        /// </summary>
        T Result { get; }
    }
}
