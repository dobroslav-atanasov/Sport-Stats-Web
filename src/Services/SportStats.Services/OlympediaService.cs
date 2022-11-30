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