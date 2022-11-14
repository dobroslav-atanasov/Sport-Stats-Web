namespace SportStats.Common.Converters.WorldCountries;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class WorldCountryConverter : BaseConverter
{
    private readonly ICountriesService countriesService;
    private readonly IHttpService httpService;

    public WorldCountryConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, ICountriesService countryService, IHttpService httpService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService)
    {
        this.countriesService = countryService;
        this.httpService = httpService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var header = document
                .DocumentNode
                .SelectSingleNode("//h1")
                .InnerText;

            var name = this.RegexService.Match(header, @"Flag of (.*)").Groups[1].Value.Trim();
            var country = new WorldCountry { Name = name, CreatedOn = DateTime.UtcNow };

            var rows = document
                .DocumentNode
                .SelectNodes("//table[@class='table-dl']//tr");

            foreach (var row in rows)
            {
                var thTag = row.Elements("th").Single().InnerText.Trim();
                var tdTag = row.Elements("td").Single().InnerText.Trim();

                switch (thTag.ToLower())
                {
                    case "independent":
                        country.IsIndependent = tdTag.ToLower() == "yes";
                        break;
                    case "country codes":
                        var countryCodeMatch = this.RegexService.Match(tdTag, @"([A-Z]{2}),\s*([A-Z]{3})");
                        if (countryCodeMatch != null)
                        {
                            country.TwoDigitsCode = countryCodeMatch.Groups[1].Value;
                            country.Code = countryCodeMatch.Groups[2].Value;
                        }
                        else
                        {
                            countryCodeMatch = this.RegexService.Match(tdTag, @"([A-Z-]{6})");
                            if (countryCodeMatch != null)
                            {
                                country.Code = countryCodeMatch.Groups[1].Value;
                            }
                        }
                        break;
                    case "official name":
                        country.OfficialName = tdTag;
                        break;
                    case "capital city":
                        country.Capital = tdTag;
                        break;
                    case "continent":
                        country.Continent = tdTag;
                        break;
                    case "member of":
                        country.MemberOf = tdTag;
                        break;
                    case "population":
                        var populationMatch = this.RegexService.Match(tdTag, @"([\d\s]+)\(([\d]{4})\)");
                        if (populationMatch != null)
                        {
                            var text = populationMatch.Groups[1].Value.Trim();
                            text = this.RegexService.Replace(text, @"\s*", string.Empty);
                            country.Population = int.Parse(text);
                        }
                        break;
                    case "total area":
                        var areaMatch = this.RegexService.Match(tdTag, @"([\d\s]+)km");
                        if (areaMatch != null)
                        {
                            var text = areaMatch.Groups[1].Value.Trim();
                            text = this.RegexService.Replace(text, @"\s*", string.Empty);
                            country.TotalArea = int.Parse(text);
                        }
                        break;
                    case "highest point":
                        var highestPointMatch = this.RegexService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                        if (highestPointMatch != null)
                        {
                            country.HighestPointPlace = highestPointMatch.Groups[1].Value.Trim();
                            var text = highestPointMatch.Groups[2].Value.Trim();
                            text = this.RegexService.Replace(text, @"\s*", string.Empty);
                            country.HighestPoint = int.Parse(text);
                        }
                        break;
                    case "lowest point":
                        var lowestPointMatch = this.RegexService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                        if (lowestPointMatch != null)
                        {
                            country.LowestPointPlace = lowestPointMatch.Groups[1].Value.Trim();
                            var text = lowestPointMatch.Groups[2].Value.Trim();
                            text = this.RegexService.Replace(text, @"\s*", string.Empty);
                            country.LowestPoint = int.Parse(text);
                        }
                        break;
                }
            }

            var coutnryCode = country.TwoDigitsCode != null ? country.TwoDigitsCode.ToLower() : country.Code.ToLower();
            var flag = await this.httpService.DownloadBytesAsync($"{CrawlerConstants.WORLD_COUNTRIES_DOWNLOAD_IMAGE}{coutnryCode}.png");
            country.Flag = flag;

            var dbCountry = await this.countriesService.GetWorldCountryAsync(country.Code);
            if (dbCountry == null)
            {
                await this.countriesService.AddAsync(country);
                this.Logger.LogInformation($"Added country: {country.Name}");
            }
            else
            {
                if (dbCountry.Update(country))
                {
                    dbCountry.ModifiedOn = DateTime.UtcNow;
                    await this.countriesService.UpdateAsync(dbCountry);
                    this.Logger.LogInformation($"Updated country: {country.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}