namespace SportStats.Data.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SportStats.Data.Models.Entities.SportStats;

public class SportStatsDbContext : IdentityDbContext
{
    public SportStatsDbContext(DbContextOptions<SportStatsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WorldCountry> Countries { get; set; }

    public virtual DbSet<OGCountry> OGCountries { get; set; }

    public virtual DbSet<OGGame> OGGames { get; set; }
}