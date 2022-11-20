namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class EventConverter : BaseOlympediaConverter
{
    private readonly IEventsService eventsService;

    public EventConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, IEventsService eventsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService)
    {
        this.eventsService = eventsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.OrderBy(d => d.Order).FirstOrDefault());
            var ogGameCache = this.FindGame(document);
            var ogDisciplineCache = this.FindDiscipline(document);
            if (ogGameCache != null && ogDisciplineCache != null)
            {

            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}