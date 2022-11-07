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

    public WorldCountryConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        ICountryService countryService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.countryService = countryService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var document = this.CreateHtmlDocument(group.Documents.Single());
        var header = document
            .DocumentNode
            .SelectSingleNode("//h1")
            .InnerText;

        var country = new Country();

        //await this.countryService.AddAsync(country);
    }
}