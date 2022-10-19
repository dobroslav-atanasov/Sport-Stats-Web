namespace SportStats.Data.Contexts;

using Microsoft.EntityFrameworkCore;

using SportStats.Data.Models.Entities.Crawlers;

public class CrawlerDbContext : DbContext
{
    public CrawlerDbContext(DbContextOptions<CrawlerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Crawler> Crawlers { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }
}