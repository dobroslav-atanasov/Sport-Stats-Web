namespace SportStats.Services.Data.CrawlerStorage.Interfaces;

using global::SportStats.Data.Models.Entities.Crawlers;

public interface ILogsService
{
    Task AddLogAsync(Log log);

    Task UpdateLogAsync(Guid identifier, int operation);

    Task<IEnumerable<Guid>> GetLogIdentifiersAsync(int crawlerId);
}