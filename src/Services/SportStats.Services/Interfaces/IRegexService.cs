namespace SportStats.Services.Interfaces;

using System.Text.RegularExpressions;

public interface IRegexService
{
    string MatchFirstGroup(string text, string pattern);

    Match Match(string text, string pattern);

    string Replace(string text, string pattern, string replacement);
}