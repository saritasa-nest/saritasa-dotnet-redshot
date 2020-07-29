using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RedShot.Helpers
{
    public static class UrlHelper
    {
        public static readonly char[] BidiControlCharacters = new char[] { '\u200E', '\u200F', '\u202A', '\u202B', '\u202C', '\u202D', '\u202E' };

        public static string CombineURL(string url1, string url2)
        {
            bool url1Empty = string.IsNullOrEmpty(url1);
            bool url2Empty = string.IsNullOrEmpty(url2);

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

        public static string CombineURL(params string[] urls)
        {
            return urls.Aggregate(CombineURL);
        }  

        public static string GetFileName(string path)
        {
            if (path.Contains('/'))
            {
                path = path.Substring(path.LastIndexOf('/') + 1);
            }

            if (path.Contains('?'))
            {
                path = path.Remove(path.IndexOf('?'));
            }

            if (path.Contains('#'))
            {
                path = path.Remove(path.IndexOf('#'));
            }

            return path;
        }

        public static bool IsFileURL(string url)
        {
            int index = url.LastIndexOf('/');

            if (index < 0)
            {
                return false;
            }

            string path = url.Substring(index + 1);

            return !string.IsNullOrEmpty(path) && path.Contains(".");
        }

        public static string GetDirectoryPath(string path)
        {
            if (path.Contains("/"))
            {
                path = path.Substring(0, path.LastIndexOf('/'));
            }

            return path;
        }

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

        private static readonly string[] URLPrefixes = new string[] { "http://", "https://", "ftp://", "ftps://", "file://", "//" };

        public static bool HasPrefix(string url)
        {
            return URLPrefixes.Any(x => url.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string FixPrefix(string url, string prefix = "http://")
        {
            if (!string.IsNullOrEmpty(url) && !HasPrefix(url))
            {
                return prefix + url;
            }

            return url;
        }

        public static string ForcePrefix(string url, string prefix = "https://")
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = prefix + RemovePrefixes(url);
            }

            return url;
        }

        public static string RemovePrefixes(string url)
        {
            foreach (string prefix in URLPrefixes)
            {
                if (url.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    url = url.Remove(0, prefix.Length);
                    break;
                }
            }

            return url;
        }

        public static string GetHostName(string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                string host = uri.Host;

                if (!string.IsNullOrEmpty(host))
                {
                    if (host.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
                    {
                        host = host.Substring(4);
                    }

                    return host;
                }
            }

            return url;
        }

        public static string RemoveQueryString(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                int index = url.IndexOf("?");

                if (index > -1)
                {
                    return url.Remove(index);
                }
            }

            return url;
        }

        public static string BuildUri(string root, string path, string query = null)
        {
            UriBuilder builder = new UriBuilder(root);
            builder.Path = path;
            builder.Query = query;
            return builder.Uri.AbsoluteUri;
        }
    }
}
