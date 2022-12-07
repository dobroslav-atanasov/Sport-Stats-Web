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

    public virtual DbSet<OGSport> OGSports { get; set; }

    public virtual DbSet<OGDiscipline> OGDisciplines { get; set; }

    public virtual DbSet<OGVenue> OGVenues { get; set; }

    public virtual DbSet<OGEvent> OGEvents { get; set; }

    public virtual DbSet<OGEventVenue> OGEventVenues { get; set; }

    public virtual DbSet<OGAthlete> OGAthletes { get; set; }

    public virtual DbSet<OGAthleteCountry> OGAthleteCountries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OGEventVenue>()
            .HasKey(ev => new { ev.EventId, ev.VenueId });

        modelBuilder.Entity<OGAthleteCountry>()
            .HasKey(ac => new { ac.AthleteId, ac.CountryId });

        base.OnModelCreating(modelBuilder);
    }
}