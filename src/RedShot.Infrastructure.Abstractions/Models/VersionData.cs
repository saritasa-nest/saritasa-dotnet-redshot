using System;

namespace RedShot.Infrastructure.Abstractions.Models
{
    /// <summary>
    /// Version data.
    /// </summary>
    public class VersionData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <param name="releasePageUrl">Release page URL.</param>
        /// <param name="zipFileUrl">Zip file download URL.</param>
        public VersionData(Version version, string releasePageUrl, string zipFileUrl)
        {
            Version = version;
            ReleasePageUrl = releasePageUrl;
            ZipFileUrl = zipFileUrl;
        }

        /// <summary>
        /// Version.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Release page URL.
        /// </summary>
        public string ReleasePageUrl { get; }

        /// <summary>
        /// Zip file download URL.
        /// </summary>
        public string ZipFileUrl { get; }
    }
}
