namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class NOCConverter : BaseOlympediaConverter
{
    private readonly ICountriesService countriesService;
    private readonly INormalizeService normalizeService;

    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, ICountriesService countriesService, INormalizeService normalizeService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService)
    {
        this.countriesService = countriesService;
        this.normalizeService = normalizeService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var country = new OGCountry { CreatedOn = DateTime.UtcNow };
            var countryDocument = this.CreateHtmlDocument(group.Documents.Single(d => d.Order == 1));
            var header = countryDocument.DocumentNode.SelectSingleNode("//h1");
            var match = this.RegexService.Match(header.InnerText, @"(.*?)\((.*?)\)");
            if (match != null)
            {
                country.CountryName = match.Groups[1].Value.Decode().Trim();
                country.Code = match.Groups[2].Value.Decode().Trim().ToUpper();
                country.RelatedCountry = this.FindRelatedCountry(country.Code);
            }

            if (country.Code != null && country.Code != "UNK" && country.Code != "CRT")
            {
                var countryDescription = countryDocument
                    .DocumentNode
                    .SelectSingleNode("//div[@class='description']")
                    .OuterHtml
                    .Decode();

                country.CountryDescription = this.RegexService.CutHtml(countryDescription);

                if (group.Documents.Count > 1)
                {
                    var committeeDocument = this.CreateHtmlDocument(group.Documents.Single(d => d.Order == 2));
                    var committeeName = committeeDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Decode();
                    country.CommitteeName = committeeName;

                    var abbreavition = this.RegexService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Abbreviation<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                    var foundedYear = this.RegexService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Founded<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");
                    var disbandedYear = this.RegexService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Disbanded<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");
                    var recognizedYear = this.RegexService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Recognized by the IOC<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");

                    country.CommitteeAbbreavition = !string.IsNullOrEmpty(abbreavition) ? abbreavition : null;
                    country.CommitteeFoundedYear = !string.IsNullOrEmpty(foundedYear) ? int.Parse(foundedYear) : null;
                    country.CommitteeDisbandedYear = !string.IsNullOrEmpty(disbandedYear) ? int.Parse(disbandedYear) : null;
                    country.CommitteeRecognizedYear = !string.IsNullOrEmpty(recognizedYear) ? int.Parse(recognizedYear) : null;

                    var committeeDescription = committeeDocument
                        .DocumentNode
                        .SelectSingleNode("//div[@class='description']")?
                        .OuterHtml?
                        .Decode();

                    country.CommitteeDescription = !string.IsNullOrEmpty(committeeDescription) ? this.RegexService.CutHtml(committeeDescription) : null;
                }

                var worldCountryCode = this.normalizeService.MapOlympicGamesCountriesAndWorldCountries(country.Code);
                if (worldCountryCode != null)
                {
                    var worldCountry = await this.countriesService.GetWorldCountryAsync(worldCountryCode);
                    country.CountryFlag = worldCountry.Flag;
                }

                var dbCountry = await this.countriesService.GetOlympicGameCountryAsync(country.Code);
                if (dbCountry == null)
                {
                    await this.countriesService.AddAsync(country);
                    this.Logger.LogInformation($"Added country: {country.CountryName}");
                }
                else
                {
                    if (dbCountry.Update(country))
                    {
                        dbCountry.ModifiedOn = DateTime.UtcNow;
                        await this.countriesService.UpdateAsync(dbCountry);
                        this.Logger.LogInformation($"Updated country: {country.CountryName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    public string FindRelatedCountry(string code)
    {
        string relatedCountryCode = null;
        switch (code)
        {
            case "ANZ":
                relatedCountryCode = "AUS";
                break;
            case "TCH":
            case "BOH":
                relatedCountryCode = "CZE";
                break;
            case "GDR":
            case "FRG":
            case "SAA":
                relatedCountryCode = "GER";
                break;
            case "MAL":
            case "NBO":
                relatedCountryCode = "MAS";
                break;
            case "AHO":
                relatedCountryCode = "NED";
                break;
            case "ROC":
            case "EUN":
            case "URS":
                relatedCountryCode = "RUS";
                break;
            case "YUG":
            case "SCG":
                relatedCountryCode = "SRB";
                break;
            case "YMD":
            case "YAR":
                relatedCountryCode = "YEM";
                break;
        }

        return relatedCountryCode;
    }
}