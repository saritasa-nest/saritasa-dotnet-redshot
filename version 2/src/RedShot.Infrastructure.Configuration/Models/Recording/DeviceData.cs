namespace RedShot.Infrastructure.Configuration.Models.Recording
{
    /// <summary>
    /// Contains data about device parameters.
    /// </summary>
    public class DeviceData
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Compatible FFmpeg name.
        /// </summary>
        public string CompatibleFfmpegName { get; set; }
    }
}
