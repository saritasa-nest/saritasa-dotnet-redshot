using System.Collections.Generic;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Helper for working with Urls and paths.
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Combine two urls.
        /// </summary>
        public static string CombineUrl(string url1, string url2)
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
                url1 = url1.Substring(0, url1.Length - 1);
            }

            if (url2.StartsWith("/"))
            {
                url2 = url2.Remove(0, 1);
            }

            return url1 + "/" + url2;
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
        public static List<string> GetPaths(string path)
        {
            List<string> paths = new List<string>();

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    string currentPath = path.Remove(i);

                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        paths.Add(currentPath);
                    }
                }
                else if (i == path.Length - 1)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }
    }
}
