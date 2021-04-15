using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using Saritasa.Tools.Domain.Exceptions;
using RedShot.Infrastructure.Configuration.Models;
using RedShot.Infrastructure.Abstractions.Models;
using RedShot.Infrastructure.Abstractions.Updating;

namespace RedShot.Infrastructure.Updating
{
    /// <inheritdoc cref="IApplicationVersionRepository"/>
    public class GithubApplicationVersionRepository : IApplicationVersionRepository
    {
        private const string GitHubApi = "https://api.github.com";

        private readonly GithubRepositoryDetails githubRepositoryDetails;

        /// <summary>
        /// Releases API URL.
        /// </summary>
        private string ReleasesUrl => $"repos/{githubRepositoryDetails.OwnerName}/{githubRepositoryDetails.RepositoryName}/releases";

        /// <summary>
        /// Constructor.
        /// </summary>
        public GithubApplicationVersionRepository()
        {
            githubRepositoryDetails = AppSettings.Instance.GithubRepositoryDetails;
        }

        /// <inheritdoc/>
        public async Task<VersionData> GetLatestVersionAsync(CancellationToken cancellationToken)
        {
            var client = new RestClient(GitHubApi);
            var request = new RestRequest(ReleasesUrl, Method.GET);

            var result = await client.ExecuteAsync(request, cancellationToken);

            if (!result.IsSuccessful)
            {
                throw new DomainException(result.ErrorMessage);
            }

            var releases = DeserializeReleaseContent(result.Content);
            var versions = ConvertToVersionData(releases);

            return GetLatestVersion(versions);
        }

        private static IEnumerable<VersionData> ConvertToVersionData(IEnumerable<ReleaseDetails> releases)
        {
            var versions = new List<VersionData>();

            foreach (var release in releases)
            {
                if (Version.TryParse(release.Name, out var version))
                {
                    var versionData = new VersionData(version, release.ReleasePageUrl, release.ZipFileUrl);
                    versions.Add(versionData);
                }
            }

            return versions;
        }

        private static IEnumerable<ReleaseDetails> DeserializeReleaseContent(string content)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ReleaseDetails>>(content);
        }

        private static VersionData GetLatestVersion(IEnumerable<VersionData> versions)
        {
            return versions.OrderByDescending(v => v.Version).First();
        }

        /// <summary>
        /// Release details.
        /// </summary>
        private class ReleaseDetails
        {
            /// <summary>
            /// Name.
            /// </summary>
            [JsonProperty("tag_name")]
            public string Name { get; set; }

            /// <summary>
            /// Release page URL.
            /// </summary>
            [JsonProperty("html_url")]
            public string ReleasePageUrl { get; set; }

            /// <summary>
            /// Zip file URL.
            /// </summary>
            [JsonProperty("zipball_url")]
            public string ZipFileUrl { get; set; }
        }
    }
}
