namespace SportStats.Services;

using System.Text.RegularExpressions;

using SportStats.Common;
using SportStats.Services.Interfaces;

public class RegexService : IRegexService
{
    public Match Match(string text, string pattern)
    {
        var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            return match;
        }

        return null;
    }

    public string MatchFirstGroup(string text, string pattern)
    {
        var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            var result = match.Groups[1].Value.Trim().Decode();
            return result;
        }

        return null;
    }

    public string Replace(string text, string pattern, string replacement)
    {
        return Regex.Replace(text, pattern, replacement);
    }
}