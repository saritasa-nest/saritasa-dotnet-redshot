using AutoMapper;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Contains a shared mapper instance.
    /// </summary>
    // TODO: remove this class, use dependency injection instead.
    public static class Mapping
    {
        /// <summary>
        /// Mapper to be used for mapping in the application.
        /// </summary>
        public static IMapper Mapper { get; set; }
    }
}
