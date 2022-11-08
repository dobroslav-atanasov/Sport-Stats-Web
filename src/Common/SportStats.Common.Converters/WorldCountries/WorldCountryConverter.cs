namespace SportStats.Common.Converters.WorldCountries;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class WorldCountryConverter : BaseConverter
{
    private readonly ICountryService countryService;
    private readonly IRegexService regexService;

    public WorldCountryConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        ICountryService countryService, IRegexService regexService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.countryService = countryService;
        this.regexService = regexService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var document = this.CreateHtmlDocument(group.Documents.Single());
        var header = document
            .DocumentNode
            .SelectSingleNode("//h1")
            .InnerText;

        var name = this.regexService.Match(header, @"Flag of (.*)").Groups[1].Value.Trim();
        var country = new Country { Name = name };

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
                    var countryCodeMatch = this.regexService.Match(tdTag, @"([A-Z]{2}),\s*([A-Z]{3})");
                    if (countryCodeMatch != null)
                    {
                        country.Alpha2Code = countryCodeMatch.Groups[1].Value;
                        country.Alpha3Code = countryCodeMatch.Groups[2].Value;
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
                    var populationMatch = this.regexService.Match(tdTag, @"([\d\s]+)\(([\d]{4})\)");
                    if (populationMatch != null)
                    {
                        var text = populationMatch.Groups[1].Value.Trim();
                        text = this.regexService.Replace(text, @"\s*", string.Empty);
                        country.Population = int.Parse(text);
                    }
                    break;
                case "total area":
                    var areaMatch = this.regexService.Match(tdTag, @"([\d\s]+)km");
                    if (areaMatch != null)
                    {
                        var text = areaMatch.Groups[1].Value.Trim();
                        text = this.regexService.Replace(text, @"\s*", string.Empty);
                        country.TotalArea = int.Parse(text);
                    }
                    break;
                case "highest point":
                    var highestPointMatch = this.regexService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                    break;
                case "lowest point":

                    break;
            }
        }

        ;

        //await this.countryService.AddAsync(country);
    }
}