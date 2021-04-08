using RedShot.Infrastructure.Abstractions;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Contains a shared <see cref="IApplicationUpdatingService"/> instance.
    /// </summary>
    // TODO: remove this class, use dependency injection instead.
    public static class Updating
    {
        /// <summary>
        /// Application update service.
        /// </summary>
        public static IApplicationUpdatingService ApplicationUpdateService { get; set; }
    }
}
