using RedShot.Infrastructure.Abstractions.Updating;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Contains a shared <see cref="IApplicationUpdatingService"/> instance.
    /// </summary>
    // TODO: remove this class, use dependency injection instead.
    public static class Updating
    {
        /// <summary>
        /// Application updating service.
        /// </summary>
        public static IApplicationUpdatingService ApplicationUpdatingService { get; set; }
    }
}
