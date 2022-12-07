namespace SportStats.Data.Factory;

using Microsoft.EntityFrameworkCore;

using SportStats.Data.Contexts;
using SportStats.Data.Factory.Interfaces;

public class DbContextFactory : IDbContextFactory
{
    private readonly DbContextOptions<CrawlerStorageDbContext> crawlerStorageOptions;
    private readonly DbContextOptions<SportStatsDbContext> sportStatsOptions;

    public DbContextFactory(DbContextOptions<CrawlerStorageDbContext> crawlerStorageOptions, DbContextOptions<SportStatsDbContext> sportStatsOptions)
    {
        this.crawlerStorageOptions = crawlerStorageOptions;
        this.sportStatsOptions = sportStatsOptions;
    }

    public CrawlerStorageDbContext CreateCrawlerStorageDbContext()
    {
        return new CrawlerStorageDbContext(crawlerStorageOptions);
    }

    public SportStatsDbContext CreateSportStatsDbContext()
    {
        return new SportStatsDbContext(sportStatsOptions);
    }
}