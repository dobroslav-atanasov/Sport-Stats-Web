namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class GameConverter : BaseConverter
{
    public GameConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
    }

    protected override Task ProcessGroupAsync(Group group)
    {
        throw new NotImplementedException();
    }
}