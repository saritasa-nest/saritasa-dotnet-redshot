using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestSharp;
using Saritasa.Tools.Domain.Exceptions;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration.Models;

namespace RedShot.Infrastructure
{
    /// <inheritdoc cref="IApplicationStorage"/>
    public class GithubApplicationStorage : IApplicationStorage
    {
        private const string GitHubApi = "https://api.github.com";
        private const string GitHub = "https://github.com";

        private readonly GithubRepositoryDetails githubRepositoryDetails;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GithubApplicationStorage()
        {
            githubRepositoryDetails = AppSettings.Instance.GithubRepositoryDetails;
        }

        /// <inheritdoc/>
        public async Task<Version> GetLatestVersionAsync(CancellationToken cancellationToken)
        {
            var url = $"repos/{githubRepositoryDetails.OwnerName}/{githubRepositoryDetails.RepositoryName}/tags";

            var client = new RestClient(GitHubApi);
            var request = new RestRequest(url, Method.GET);

            var result = await client.ExecuteAsync<IEnumerable<TagDetails>>(request);

            if (result.IsSuccessful)
            {
                return GetLatestVersion(result.Data);
            }

            throw new DomainException(result.ErrorMessage);
        }

        private static Version GetLatestVersion(IEnumerable<TagDetails> tagDetails)
        {
            var versions = tagDetails.Select(t => Version.Parse(t.Name)).ToList();
            versions.Sort();

            return versions.Last();
        }

        /// <inheritdoc/>
        public string GetReleaseUrl(Version version)
        {
            return $"{GitHub}/{githubRepositoryDetails.OwnerName}/{githubRepositoryDetails.OwnerName}/tag/{version}";
        }

        /// <summary>
        /// Tag details.
        /// </summary>
        private class TagDetails
        {
            /// <summary>
            /// Name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
