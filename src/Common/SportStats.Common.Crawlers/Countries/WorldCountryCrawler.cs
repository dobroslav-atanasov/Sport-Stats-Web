namespace SportStats.Common.Crawlers.Countries;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class WorldCountryCrawler : BaseCrawler
{
    public WorldCountryCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService)
        : base(logger, httpService, crawlersService)
    {
    }

    public override Task StartAsync()
    {
        throw new NotImplementedException();
    }
}