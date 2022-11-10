namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class NOCConverter : BaseConverter
{
    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        ;
    }
}