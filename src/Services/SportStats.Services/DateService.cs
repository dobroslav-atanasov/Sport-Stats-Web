namespace SportStats.Services;

using System;

using SportStats.Common;
using SportStats.Services.Interfaces;

public class DateService : IDateService
{
    private readonly IRegexService regexService;

    public DateService(IRegexService regexService)
    {
        this.regexService = regexService;
    }

    public DateTime? MatchDate(string text)
    {
        var match = this.regexService.Match(text, @"(\d+)\s*(January|February|March|April|May|June|July|August|September|October|November|December)\s*(\d{4})");
        if (match != null)
        {
            var day = int.Parse(match.Groups[1].Value);
            var month = match.Groups[2].Value.GetMonthNumber();
            var year = int.Parse(match.Groups[3].Value);
            return new DateTime(year, month, day);
        }

        return null;
    }

    public Tuple<DateTime?, DateTime?> MatchStartAndEndDate(string text)
    {
        text = text.Replace("–", string.Empty);
        var match = this.regexService.Match(text, @"(\d+)\s*([A-z]+)?\s*(\d+)?\s*([A-z]+)?\s*(\d{4})");
        if (match != null)
        {
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (match.Groups[1].Value.Length > 0 && match.Groups[2].Value.Length > 0 && match.Groups[3].Value.Length > 0 && match.Groups[4].Value.Length > 0)
            {
                startDate = DateTime.ParseExact($"{match.Groups[1].Value}-{match.Groups[2].Value.GetMonthNumber()}-{match.Groups[5].Value}", "d-M-yyyy", null);
                endDate = DateTime.ParseExact($"{match.Groups[3].Value}-{match.Groups[4].Value.GetMonthNumber()}-{match.Groups[5].Value}", "d-M-yyyy", null);
            }
            else if (match.Groups[1].Value.Length > 0 && match.Groups[3].Value.Length > 0 && match.Groups[4].Value.Length > 0)
            {
                startDate = DateTime.ParseExact($"{match.Groups[1].Value}-{match.Groups[4].Value.GetMonthNumber()}-{match.Groups[5].Value}", "d-M-yyyy", null);
                endDate = DateTime.ParseExact($"{match.Groups[3].Value}-{match.Groups[4].Value.GetMonthNumber()}-{match.Groups[5].Value}", "d-M-yyyy", null);
            }
            else if (match.Groups[1].Value.Length > 0 && match.Groups[2].Value.Length > 0)
            {
                startDate = DateTime.ParseExact($"{match.Groups[1].Value}-{match.Groups[2].Value.GetMonthNumber()}-{match.Groups[5].Value}", "d-M-yyyy", null);
            }

            return new Tuple<DateTime?, DateTime?>(startDate, endDate);
        }

        return null;
    }
}