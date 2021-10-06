namespace RedShot.Infrastructure.Configuration.Models
{
    /// <summary>
    /// Application settings model.
    /// </summary>
    public class AppSettings
    {
        private static AppSettings instance;

        /// <summary>
        /// Instance of application settings.
        /// </summary>
        public static AppSettings Instance
        {
            get
            {
                return instance;
            }

            set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }

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
