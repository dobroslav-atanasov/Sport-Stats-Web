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
    private readonly IDateService dateService;
    private readonly IEventVenueService eventVenueService;

    public EventConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IOlympediaService olympediaService, IEventsService eventsService,
        IDateService dateService, IEventVenueService eventVenueService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService, olympediaService)
    {
        this.eventsService = eventsService;
        this.dateService = dateService;
        this.eventVenueService = eventVenueService;
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
            if (eventModel != null && !this.CheckForbiddenEvent(eventModel.OriginalName, ogDisciplineCache.Name, ogGameCache.Year))
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

                var dateMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Date<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (dateMatch != null)
                {
                    var dates = this.dateService.MatchStartAndEndDate(dateMatch.Groups[1].Value.Trim());
                    if (dates != null)
                    {
                        @event.StartDate = dates.Item1;
                        @event.EndDate = dates.Item2;
                    }
                }

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

                await this.ProcessEventVenueAsync(document, @event, dbEvent);
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private async Task ProcessEventVenueAsync(HtmlDocument document, OGEvent @event, OGEvent dbEvent)
    {
        var locationMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Location<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
        var eventId = dbEvent != null ? dbEvent.Id : @event.Id;
        if (locationMatch != null)
        {
            var venues = this.OlympediaService.FindVenues(locationMatch.Groups[1].Value);

            foreach (var venue in venues)
            {
                var venueCache = this.DataCacheService.VenueCacheModels.FirstOrDefault(v => v.Number == venue);
                if (venueCache != null && !this.eventVenueService.EventVenueExists(@event.Id, venueCache.Id))
                {
                    await this.eventVenueService.AddAsync(new OGEventVenue { EventId = eventId, VenueId = venueCache.Id });
                    this.Logger.LogInformation($"Added event: {eventId} and venue: {venueCache.Id}");
                }
            }
        }
    }

    private void IsTeamEvent(HtmlDocument document, OGEvent @event)
    {
        var table = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var rows = table.Elements("tr");

        var athletes = this.OlympediaService.FindAthleteNumbers(table.OuterHtml);
        var codes = this.OlympediaService.FindCountryCodes(table.OuterHtml);

        if (athletes.Count != codes.Count)
        {
            @event.IsTeamEvent = true;
        }

        if (@event.NormalizedName.ToLower().Contains("individual"))
        {
            @event.IsTeamEvent = false;
        }
    }
}