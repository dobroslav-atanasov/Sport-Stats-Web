namespace SportStats.Common.Converters.Olympedia;

using System;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class AthleteConverter : BaseOlympediaConverter
{
    private readonly IAthletesService athletesService;
    private readonly IDateService dateService;
    private readonly IOlympediaService olympediaService;
    private readonly IAthleteCountryService athleteCountryService;

    public AthleteConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IAthletesService athletesService, IDateService dateService,
        IOlympediaService olympediaService, IAthleteCountryService athleteCountryService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService)
    {
        this.athletesService = athletesService;
        this.dateService = dateService;
        this.olympediaService = olympediaService;
        this.athleteCountryService = athleteCountryService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var number = int.Parse(new Uri(group.Documents.Single().Url).Segments.Last());
            var name = document.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();
            var athlete = new OGAthlete
            {
                CreatedOn = DateTime.UtcNow,
                Name = name,
                Number = number,
                Identifier = Guid.NewGuid(),
                EnglishName = this.NormalizeService.ReplaceNonEnglishLetters(name)
            };

            var typeMatch = this.RegexService.Match(document.ParsedText, @"<tr>\s*<th>Roles<\/th>\s*<td>(.*?)<\/td><\/tr>");
            if (typeMatch != null)
            {
                athlete.Type = this.NormalizeService.MapAthleteType(typeMatch.Groups[1].Value);
            }

            var genderMatch = this.RegexService.Match(document.ParsedText, @"<tr>\s*<th>Sex<\/th>\s*<td>(Male|Female)<\/td><\/tr>");
            if (genderMatch != null)
            {
                athlete.Gender = genderMatch.Groups[1].Value.UpperFirstChar().ToEnum<GenderType>();
            }

            athlete.FullName = this.RegexService.MatchFirstGroup(document.ParsedText, @"<tr>\s*<th>Full name<\/th>\s*<td>(.*?)<\/td><\/tr>")?.Replace("•", " ");
            athlete.Association = this.ExtractAssociations(document.ParsedText);
            athlete.Description = this.RegexService.CutHtml(this.RegexService.MatchFirstGroup(document.ParsedText, @"<div class=(?:""|')description(?:""|')>(.*?)<\/div>"));
            athlete.Nationality = this.ExtractNationality(document.ParsedText);

            var bornMatch = this.RegexService.Match(document.ParsedText, @"<tr>\s*<th>Born<\/th>\s*<td>(.*?)<\/td><\/tr>");
            if (bornMatch != null)
            {
                athlete.BirthDate = this.dateService.MatchDate(bornMatch.Groups[1].Value);
                athlete.BirthPlace = this.RegexService.MatchFirstGroup(bornMatch.Groups[1].Value, @"<a href=(?:""|')\/place_names\/[\d]+(?:""|')>(.*?)<\/a>");
            }

            var diedMatch = this.RegexService.Match(document.ParsedText, @"<tr>\s*<th>Died<\/th>\s*<td>(.*?)<\/td><\/tr>");
            if (diedMatch != null)
            {
                athlete.DiedDate = this.dateService.MatchDate(diedMatch.Groups[1].Value);
                athlete.DiedPlace = this.RegexService.MatchFirstGroup(diedMatch.Groups[1].Value, @"<a href=(?:""|')\/place_names\/[\d]+(?:""|')>(.*?)<\/a>");
            }

            var measurmentsMatch = this.RegexService.Match(document.ParsedText, @"<tr>\s*<th>Measurements<\/th>\s*<td>(.*?)<\/td><\/tr>");
            if (measurmentsMatch != null)
            {
                var heightMatch = this.RegexService.Match(measurmentsMatch.Groups[1].Value, @"([\d]+)\s*cm");
                if (heightMatch != null)
                {
                    athlete.Height = int.Parse(heightMatch.Groups[1].Value);
                }

                var weightMatch = this.RegexService.Match(measurmentsMatch.Groups[1].Value, @"([\d]+)\s*kg");
                if (weightMatch != null)
                {
                    athlete.Weight = int.Parse(weightMatch.Groups[1].Value);
                }
            }

            var dbAthlete = await this.athletesService.GetAthleteByNumberAsync(athlete.Number);
            if (dbAthlete == null)
            {
                await this.athletesService.AddAsync(athlete);
                this.Logger.LogInformation($"Added athlete: {athlete.Number}");
            }
            else
            {
                if (dbAthlete.Update(athlete))
                {
                    dbAthlete.ModifiedOn = DateTime.UtcNow;
                    await this.athletesService.UpdateAsync(dbAthlete);
                    this.Logger.LogInformation($"Updated athlete: {athlete.Number}");
                }
            }

            await this.ProcessAthleteCountryAsync(document, athlete, dbAthlete);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private async Task ProcessAthleteCountryAsync(HtmlDocument document, OGAthlete athlete, OGAthlete dbAthlete)
    {
        var nocMatch = this.RegexService.Match(document.ParsedText, @"<tr>\s*<th>NOC(?:\(s\))?<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
        var athleteId = dbAthlete != null ? dbAthlete.Id : athlete.Id;
        if (nocMatch != null)
        {
            var countryCodes = this.olympediaService.FindCountryCodes(nocMatch.Groups[1].Value);
            foreach (var code in countryCodes)
            {
                var countryCache = this.DataCacheService.OGCountriesCache.FirstOrDefault(c => c.Code == code);
                if (countryCache != null && !this.athleteCountryService.AthleteCountryExists(athleteId, countryCache.Id))
                {
                    await this.athleteCountryService.AddAsync(new OGAthleteCountry { AthleteId = athleteId, CountryId = countryCache.Id });
                    this.Logger.LogInformation($"Added athlete: {athleteId} and country: {code}");
                }
            }
        }
    }

    private string ExtractNationality(string text)
    {
        var match = this.RegexService.Match(text, @"<tr>\s*<th>Nationality<\/th>\s*<td>(.*?)<\/td><\/tr>");
        if (match != null)
        {
            var countryMatch = this.RegexService.Match(match.Groups[1].Value, @"<a href=""/countries/(.*?)"">(.*?)</a>");
            if (countryMatch != null)
            {
                return countryMatch.Groups[2].Value.Trim();
            }
        }

        return null;
    }

    private string ExtractAssociations(string text)
    {
        var associationMatch = this.RegexService.Match(text, @"<tr>\s*<th>Affiliations<\/th>\s*<td>(.*?)<\/td><\/tr>");
        if (associationMatch != null)
        {
            var matches = this.RegexService.Matches(associationMatch.Groups[1].Value, @"<a href=""/affiliations/(\d+)"">(.*?)</a>");
            if (matches != null && matches.Count != 0)
            {
                var result = string.Join("|", matches.Select(x => x.Groups[2].Value.Trim()).ToList());
                return result;
            }
            else
            {
                return associationMatch.Groups[1].Value.Trim();
            }
        }

        return null;
    }
}