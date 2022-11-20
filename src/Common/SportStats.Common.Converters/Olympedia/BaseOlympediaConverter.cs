namespace SportStats.Common.Converters.Olympedia;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Cache;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public abstract class BaseOlympediaConverter : BaseConverter
{
    protected BaseOlympediaConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService,
        IZipService zipService, IRegexService regexService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService)
    {
        this.DataCacheService = dataCacheService;
    }

    protected IDataCacheService DataCacheService { get; }

    protected OGGameCacheModel FindGame(HtmlDocument htmlDocument)
    {
        var headers = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
        var gameMatch = this.RegexService.Match(headers.OuterHtml, @"<a href=""\/editions\/(?:\d+)"">(\d+)\s*(\w+)\s*Olympics<\/a>");

        if (gameMatch != null)
        {
            var gameYear = int.Parse(gameMatch.Groups[1].Value);
            var gameType = gameMatch.Groups[2].Value.Trim();

            if (gameType.ToLower() == "equestrian")
            {
                gameType = "Summer";
            }

            var game = this.DataCacheService
                .OGGamesCache
                .FirstOrDefault(g => g.Year == gameYear && g.Type == gameType.ToEnum<OlympicGameType>());

            return game;
        }

        return null;
    }

    protected OGDisciplineCacheModel FindDiscipline(HtmlDocument htmlDocument)
    {
        var headers = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
        var disciplineName = this.RegexService.MatchFirstGroup(headers.OuterHtml, @"<a href=""\/editions\/[\d]+\/sports\/(?:.*?)"">(.*?)<\/a>");
        var eventName = this.RegexService.MatchFirstGroup(headers.OuterHtml, @"<li\s*class=""active"">(.*?)<\/li>");

        if (disciplineName != null && eventName != null)
        {
            if (disciplineName.ToLower() == "wrestling")
            {
                if (eventName.ToLower().Contains("freestyle"))
                {
                    disciplineName = "Wrestling Freestyle";
                }
                else
                {
                    disciplineName = "Wrestling Greco-Roman";
                }
            }

            var discipline = this.DataCacheService
                .OGDisciplinesCache
                .FirstOrDefault(d => d.Name == disciplineName);

            return discipline;
        }

        return null;
    }
}