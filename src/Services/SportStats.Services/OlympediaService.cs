namespace SportStats.Services;

using System.Collections.Generic;

using SportStats.Services.Interfaces;

public class OlympediaService : IOlympediaService
{
    private readonly IRegexService regexService;

    public OlympediaService(IRegexService regexService)
    {
        this.regexService = regexService;
    }

    public int FindAthleteNumber(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            var numberMatch = this.regexService.Match(text, @"<a href=""\/athletes\/(\d+)"">");
            if (numberMatch != null)
            {
                return int.Parse(numberMatch.Groups[1].Value);
            }
        }

        return 0;
    }

    public IList<int> FindAthleteNumbers(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<int>();
        }

        var numbers = this.regexService
            .Matches(text, @"<a href=""\/athletes\/(\d+)"">")
            .Select(x => int.Parse(x.Groups[1].Value))?
            .ToList();

        return numbers;
    }

    public string FindCountryCode(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var numberMatch = this.regexService.Match(text, @"<a href=""\/countries\/(.*?)"">");

        return numberMatch.Groups[1].Value;
    }

    public IList<string> FindCountryCodes(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<string>();
        }

        var codes = this.regexService
            .Matches(text, @"<a href=""\/countries\/(.*?)"">")
            .Select(x => x.Groups[1].Value)?
            .Where(x => x != "UNK")
            .ToList();

        return codes;
    }

    public List<int> FindVenues(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<int>();
        }

        var venues = this.regexService
            .Matches(text, @"\/venues\/(\d+)")
            .Select(x => int.Parse(x.Groups[1].Value))?
            .ToList();

        return venues;
    }
}