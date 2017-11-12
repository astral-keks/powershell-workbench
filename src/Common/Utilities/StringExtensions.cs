using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Common.Utilities
{
    public static class StringExtensions
    {
        public static StringComparison StringComparison = StringComparison.OrdinalIgnoreCase;

        public static bool Is(this string a, string b)
        {
            return string.Equals(a, b, StringComparison);
        }

        public static bool IsAny(this string a, HashSet<string> values)
        {
            if (values == null)
                return false;

            return values.Contains(a);
        }

        public static bool IsNot(this string a, string b)
        {
            return !a.Is(b);
        }

        public static bool Has(this string a, string b)
        {
            if (a == null || b == null)
                return false;

            return a.IndexOf(b, StringComparison) > -1;
        }

        public static bool HasAny(this string a, IEnumerable<string> parts)
        {
            if (a == null || parts == null)
                return false;

            return parts.Any(a.Has);
        }

        public static bool Like(this string source, string wildcard)
        {
            wildcard = !string.IsNullOrEmpty(wildcard) ? wildcard : "*";
            wildcard = wildcard.Replace("*", ".*").Replace("?", ".");
            return Regex.IsMatch(source, $"{wildcard}", RegexOptions.IgnoreCase);
        }

        public static bool Matches(this string source, string wildcard)
        {
            wildcard = !string.IsNullOrEmpty(wildcard) ? wildcard : "*";
            wildcard = wildcard.Replace("*", ".*").Replace("?", ".");
            return Regex.IsMatch(source, $"^{wildcard}$", RegexOptions.IgnoreCase);
        }

        public static bool Matches(this string a, Regex pattern)
        {
            if (a == null || pattern == null)
                return false;

            return pattern.IsMatch(a);
        }

        public static bool LikeAny(this string a, IEnumerable<string> patterns)
        {
            if (a == null || patterns == null)
                return false;

            return patterns.Any(a.Like);
        }
    }
}
