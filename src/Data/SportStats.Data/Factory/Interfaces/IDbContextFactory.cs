namespace SportStats.Data.Factory.Interfaces;

using SportStats.Data.Contexts;

public interface IDbContextFactory
{
    CrawlerStorageDbContext CreateCrawlerStorageDbContext();

    SportStatsDbContext CreateSportStatsDbContext();
}