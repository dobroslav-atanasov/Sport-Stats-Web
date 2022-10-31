namespace SportStats.Common.Crawlers.Olympedia;

using Microsoft.Extensions.Logging;

using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public abstract class BaseOlympediaCrawler : BaseCrawler
{
    protected BaseOlympediaCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, httpService, crawlersService, groupsService)
    {
    }
}