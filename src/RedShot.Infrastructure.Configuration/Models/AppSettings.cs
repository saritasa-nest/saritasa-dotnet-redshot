namespace RedShot.Infrastructure.Configuration.Models
{
    /// <summary>
    /// Application settings model.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// RedShot email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// FFmpeg download path for windows.
        /// </summary>
        public string FfmpegWindowsDownloadPath { get; set; }

        /// <summary>
        /// Format filename tag.
        /// </summary>
        public string FormatFileNameTag { get; set; }
    }
}
