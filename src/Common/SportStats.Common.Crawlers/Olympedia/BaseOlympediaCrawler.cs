namespace SportStats.Common.Crawlers.Olympedia;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Http;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public abstract class BaseOlympediaCrawler : BaseCrawler
{
    protected BaseOlympediaCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, httpService, crawlersService, groupsService)
    {
    }

    protected IReadOnlyCollection<string> ExtractGameUrls(HttpModel httpModel)
    {
        var tables = httpModel
            .HtmlDocument
            .DocumentNode
            .SelectNodes("//table[@class='table table-striped']")
            .Take(3)
            .ToList();

        var urls = new List<string>();
        foreach (var table in tables)
        {
            var document = new HtmlDocument();
            document.LoadHtml(table.OuterHtml);

            var currentUrls = document
                .DocumentNode
                .SelectNodes("//a")
                .Select(x => x.Attributes["href"]?.Value)
                .Where(x => x != null)
                .Select(x => this.CreateUrl(x, CrawlerConstants.OLYMPEDIA_MAIN_URL))
                .Distinct()
                .ToList();

            urls.AddRange(currentUrls);
        }

        return urls;
    }

    protected IReadOnlyCollection<string> ExtractOlympediaDisciplineUrls(HttpModel httpModel)
    {
        var table = httpModel
            .HtmlDocument
            .DocumentNode
            .SelectNodes("//table[@class='table table-striped']")?
            .FirstOrDefault();

        if (table == null)
        {
            return null;
        }

        var document = new HtmlDocument();
        document.LoadHtml(table.OuterHtml);

        var disciplineUrls = document
            .DocumentNode
            .SelectNodes("//a")
            .Select(x => x.Attributes["href"]?.Value)
            .Where(x => x != null)
            .Select(x => this.CreateUrl(x, CrawlerConstants.OLYMPEDIA_MAIN_URL))
            .Distinct()
            .ToList();

        return disciplineUrls;
    }
}