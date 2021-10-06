namespace RedShot.Infrastructure.Configuration.Models.General
{
    /// <summary>
    /// General configuration.
    /// </summary>
    public class GeneralConfiguration
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GeneralConfiguration()
        {
            Pattern = "%date%rnd";
            LaunchAtSystemStart = true;
        }

        /// <summary>
        /// Pattern for formatting.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Whether to launch at system start.
        /// </summary>
        public bool LaunchAtSystemStart { get; set; }
    }
}
