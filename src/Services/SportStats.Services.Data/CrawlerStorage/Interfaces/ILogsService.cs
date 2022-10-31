namespace SportStats.Services.Data.CrawlerStorage.Interfaces;

using SportStats.Data.Models.Entities.Crawlers;

public interface ILogsService
{
    Task AddLogAsync(Log log);

    Task UpdateLogAsync(Guid identifier, int operation);
}