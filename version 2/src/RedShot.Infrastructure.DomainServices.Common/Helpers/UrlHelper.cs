using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedShot.Infrastructure.DomainServices.Common.Helpers
{
    /// <summary>
    /// Helper for working with URL's and paths.
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Combine two URLs.
        /// </summary>
        public static string CombineUrl(string url1, string url2, params string[] otherUrls)
        {
            var url1Empty = string.IsNullOrEmpty(url1);
            var url2Empty = string.IsNullOrEmpty(url2);

            if (url1Empty && url2Empty)
            {
                return "";
            }

            if (url1Empty)
            {
                return url2;
            }

            if (url2Empty)
            {
                return url1;
            }

            if (url1.EndsWith("/"))
            {
                url1 = url1[0..^1];
            }

            if (url2.StartsWith("/"))
            {
                url2 = url2.Remove(0, 1);
            }

            var combinedUrl = url1 + "/" + url2;

            if (otherUrls.Any())
            {
                return CombineUrl(combinedUrl, otherUrls.First(), otherUrls.Skip(1).ToArray());
            }

            return combinedUrl;
        }

        /// <summary>
        /// Gives directory path.
        /// </summary>
        public static string GetDirectoryPath(string path)
        {
            if (path.Contains("/"))
            {
                path = path.Substring(0, path.LastIndexOf('/'));
            }

            return path;
        }

        /// <summary>
        /// Get all paths from path string.
        /// </summary>
        public static IEnumerable<string> GetPaths(string path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    var currentPath = path.Remove(i);

                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        yield return currentPath;
                    }
                }
                else if (i == path.Length - 1)
                {
                    yield return path;
                }
            }
        }
    }
}
