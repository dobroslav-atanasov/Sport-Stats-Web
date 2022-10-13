namespace SportStats.Data.Contexts;

using Microsoft.EntityFrameworkCore;

using SportStats.Data.Models.Entities.Countries;

public class CountryDbContext : DbContext
{
    public CountryDbContext(DbContextOptions<CountryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }
}