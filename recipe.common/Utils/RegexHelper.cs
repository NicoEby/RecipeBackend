using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ch.thommenmedia.common.Utils
{
    public static class RegexHelper
    {
        /// <summary>
        ///     Gets the matches of a regex execution
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="regex">The regular expression.</param>
        /// <returns>MatchCollection</returns>
        public static MatchCollection GetMatches(string text, string regex)
        {
            var rx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.Matches(text);
        }

        public static Match GetFirstMatch(string text, string regex)
        {
            var rx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = rx.Matches(text);
            return matches.Count <= 0 ? null : matches[0];
        }


        public static List<string> GetGroups(string text, string regex, string groupName)
        {
            var matches = GetMatches(text, regex);
            if (matches.Count <= 0)
                return null;

            var output = new List<string>();
            foreach (Match match in matches)
            {
                if (match.Groups[groupName].Success)
                    output.Add(match.Groups[groupName].Value);
            }
            return output;
        }

        public static string GetGroup(string text, string regex, string groupName)
        {
            var matches = GetMatches(text, regex);
            if (matches.Count <= 0)
                return null;
            return matches[0].Groups[groupName].Value;
        }

        public static string GetGroup(string text, string regex, int groupIndex)
        {
            var matches = GetMatches(text, regex);
            if (matches.Count <= 0)
                return null;
            return matches[0].Groups[groupIndex].Value;
        }

        /// <summary>
        ///     checkt ob ein String einem regex pattern entspricht
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool Match(string input, string regex, RegexOptions options = RegexOptions.IgnoreCase)
        {
            var match = Regex.Match(input, regex, options);
            return match.Success;
        }
    }
}