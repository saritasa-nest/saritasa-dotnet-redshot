using RedShot.Infrastructure.Abstractions;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Contains a shared <see cref="IApplicationUpdateService"/> instance.
    /// </summary>
    // TODO: remove this class, use dependency injection instead.
    public static class Updating
    {
        /// <summary>
        /// Application update service.
        /// </summary>
        public static IApplicationUpdateService ApplicationUpdateService { get; set; }
    }
}
