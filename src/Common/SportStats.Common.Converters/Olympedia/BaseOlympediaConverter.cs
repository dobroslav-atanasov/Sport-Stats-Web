namespace SportStats.Common.Converters.Olympedia;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Cache;
using SportStats.Data.Models.Convert;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public abstract class BaseOlympediaConverter : BaseConverter
{
    protected BaseOlympediaConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService,
        IZipService zipService, IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService)
    {
        this.DataCacheService = dataCacheService;
        this.NormalizeService = normalizeService;
    }

    protected IDataCacheService DataCacheService { get; }

    protected INormalizeService NormalizeService { get; }

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
            else if (disciplineName.ToLower() == "canoe marathon")
            {
                disciplineName = "Canoe Sprint";
            }

            var discipline = this.DataCacheService
                .OGDisciplinesCache
                .FirstOrDefault(d => d.Name == disciplineName);

            return discipline;
        }

        return null;
    }

    protected EventModel CreateEventModel(string originalEventName, OGGameCacheModel gameCache, OGDisciplineCacheModel disciplineCache)
    {
        if (gameCache != null && disciplineCache != null)
        {
            var eventModel = new EventModel
            {
                OriginalName = originalEventName,
                GameId = gameCache.Id,
                GameYear = gameCache.Year,
                DisciplineId = disciplineCache.Id,
                DisciplineName = disciplineCache.Name,
                Name = this.NormalizeService.NormalizeEventName(originalEventName, gameCache.Year, disciplineCache.Name)
            };
            var parts = eventModel.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var gender = parts.Last().Trim();
            eventModel.Name = string.Join("|", parts.Take(parts.Count - 1).Select(x => x.Trim()).ToList());

            this.AddInfo(eventModel);

            if (disciplineCache.Name == "Wrestling Freestyle")
            {
                eventModel.Name = eventModel.Name.Replace("Freestyle", string.Empty);
            }
            else if (disciplineCache.Name == "Wrestling Greco-Roman")
            {
                eventModel.Name = eventModel.Name.Replace("Greco-Roman", string.Empty);
            }

            if (this.RegexService.IsMatch(eventModel.Name, @"Team"))
            {
                eventModel.Name = this.RegexService.Replace(eventModel.Name, @"Team", string.Empty);
                eventModel.Name = $"Team|{eventModel.Name}";
            }

            if (this.RegexService.IsMatch(eventModel.Name, @"Individual"))
            {
                eventModel.Name = this.RegexService.Replace(eventModel.Name, @"Individual", string.Empty);
                eventModel.Name = $"Individual|{eventModel.Name}";
            }

            var nameParts = eventModel.Name.Split(new[] { " ", "|" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.UpperFirstChar()).ToList();
            var name = string.Join(" ", nameParts);

            eventModel.Name = $"{gender} {name}";
            var prefix = this.ConvertEventPrefix(gender);
            eventModel.NormalizedName = $"{prefix} {name.ToLower()}";

            return eventModel;
        }

        return null;
    }

    protected bool CheckForbiddenEvent(string eventName, string disciplineName, int year)
    {
        var list = new List<string>
        {
            "1900-Archery-Unknown Event, Men",
            "1920-Shooting-Unknown Event, Men",
            "1904-Artistic Gymnastics-Individual All-Around, Field Sports, Men"
        };

        var isForbidden = list.Any(x => x == $"{year}-{disciplineName}-{eventName}");
        return isForbidden;
    }

    private string ConvertEventPrefix(string gender)
    {

        switch (gender.ToLower())
        {
            case "men":
                gender = "Men's";
                break;
            case "women":
                gender = "Women's";
                break;
            case "mixed":
            case "open":
                gender = "Mixed";
                break;
        }

        return gender;
    }

    private void AddInfo(EventModel eventModel)
    {
        var match = this.RegexService.Match(eventModel.Name, @"\(.*?\)");
        if (match != null)
        {
            var text = match.Groups[0].Value;
            eventModel.Name = eventModel.Name.Replace(text, string.Empty).Trim();

            var poundMatch = this.RegexService.Match(text, @"(\+|-)([\d\.]+)\s*pounds");
            var kilogramMatch = this.RegexService.Match(text, @"(\+|-)([\d\.]+)\s*kilograms");
            var otherMatch = this.RegexService.Match(text, @"\((.*?)\)");
            if (poundMatch != null)
            {
                var weight = double.Parse(poundMatch.Groups[2].Value).ConvertPoundToKilograms();
                eventModel.AdditionalInfo = $"{poundMatch.Groups[1].Value.Trim()}{weight.ToString("F2")}kg";
            }
            else if (kilogramMatch != null)
            {
                var weight = double.Parse(kilogramMatch.Groups[2].Value);
                eventModel.AdditionalInfo = $"{kilogramMatch.Groups[1].Value.Trim()}{weight}kg";
            }
            else if (otherMatch != null)
            {
                eventModel.AdditionalInfo = otherMatch.Value.Replace("(", string.Empty).Replace(")", string.Empty).Trim();
            }
        }
    }
}