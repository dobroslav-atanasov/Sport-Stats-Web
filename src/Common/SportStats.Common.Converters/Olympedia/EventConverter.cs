namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class EventConverter : BaseOlympediaConverter
{
    private readonly IEventsService eventsService;
    private readonly IOlympediaService olympediaService;

    public EventConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IEventsService eventsService, IOlympediaService olympediaService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService)
    {
        this.eventsService = eventsService;
        this.olympediaService = olympediaService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.OrderBy(d => d.Order).FirstOrDefault());
            var originalEventName = document.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']/li[@class='active']").InnerText;
            var ogGameCache = this.FindGame(document);
            var ogDisciplineCache = this.FindDiscipline(document);
            var eventModel = this.CreateEventModel(originalEventName, ogGameCache, ogDisciplineCache);
            if (eventModel != null)
            {
                var @event = new OGEvent
                {
                    CreatedOn = DateTime.UtcNow,
                    OriginalName = eventModel.OriginalName,
                    Name = eventModel.Name,
                    NormalizedName = eventModel.NormalizedName,
                    AdditionalInfo = eventModel.AdditionalInfo,
                    DisciplineId = ogDisciplineCache.Id,
                    GameId = ogGameCache.Id,

                };

                this.IsTeamEvent(document, @event);
                // start date

                // end date

                var format = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Format<\/th>\s*<td colspan=""2"">(.*?)<\/td>\s*<\/tr>");
                @event.Format = format;

                var description = document.DocumentNode.SelectSingleNode("//div[@class='description']")?.OuterHtml;
                @event.Description = description != null ? this.RegexService.CutHtml(description) : null;

                var dbEvent = await this.eventsService.GetEventAsync(@event.OriginalName, @event.DisciplineId, @event.GameId);
                if (dbEvent == null)
                {
                    await this.eventsService.AddAsync(@event);
                    this.Logger.LogInformation($"Added event: {@event.Name}");
                }
                else
                {
                    if (dbEvent.Update(@event))
                    {
                        dbEvent.ModifiedOn = DateTime.UtcNow;
                        await this.eventsService.UpdateAsync(dbEvent);
                        this.Logger.LogInformation($"Updated event: {dbEvent.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private void IsTeamEvent(HtmlDocument document, OGEvent @event)
    {
        var table = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var rows = table.Elements("tr");

        var athletes = this.olympediaService.FindAthleteNumbers(table.OuterHtml);
        var codes = this.olympediaService.FindCountryCodes(table.OuterHtml);

        if (athletes.Count != codes.Count)
        {
            @event.IsTeamEvent = true;
        }
    }
}