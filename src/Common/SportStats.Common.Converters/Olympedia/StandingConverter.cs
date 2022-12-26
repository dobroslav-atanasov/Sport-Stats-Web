namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class StandingConverter : BaseOlympediaConverter
{
    public StandingConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IOlympediaService olympediaService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService, olympediaService)
    {
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var document = this.CreateHtmlDocument(group.Documents.Single(x => x.Order == 1));
        var originalEventName = document.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']/li[@class='active']").InnerText;
        var game = this.FindGame(document);
        var discipline = this.FindDiscipline(document);
        var eventModel = this.CreateEventModel(originalEventName, game, discipline);
        if (eventModel != null && !this.CheckForbiddenEvent(eventModel.OriginalName, discipline.Name, game.Year))
        {
            var @event = this.DataCacheService
                    .EventCacheModels
                    .FirstOrDefault(x => x.OriginalName == eventModel.OriginalName && x.GameId == eventModel.GameId && x.DisciplineId == eventModel.DisciplineId);

            var trRows = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']").Elements("tr");
            if (@event.IsTeamEvent)
            {

            }
            else
            {

            }
        }
    }
}