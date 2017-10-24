using System;
using System.IO;

namespace AstralKeks.Workbench.Common.Utilities
{
    public static class PathBuilder
    {
        public static string Complete(string source, params string[] extras)
        {
            var result = source;

            if (result != null)
            {
                foreach (var extra in extras)
                    result = Complete(result, extra);
            }

            return result;
        }

        public static string Complete(string source, string extra)
        {
            var result = source;

            if (result != null)
                result = Combine(result, extra);

            return result;
        }

        public static string Combine(string path1, params string[] pathN)
        {
            var result = Bare(path1);

            foreach (var path in pathN)
                result = Combine(result, path);

            return result;
        }

        public static string Combine(string path1, string path2)
        {
            return Path.Combine(Bare(path1), Bare(path2));
        }

        public static string Bare(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = string.Empty;
            return path;
        }
    }
}
