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
    public virtual DbSet<OGParticipant> OGParticipants { get; set; }
    public virtual DbSet<OGTeam> OGTeams { get; set; }
    public virtual DbSet<OGSquad> OGSquads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OGEventVenue>()
            .HasKey(ev => new { ev.EventId, ev.VenueId });

        modelBuilder.Entity<OGAthleteCountry>()
            .HasKey(ac => new { ac.AthleteId, ac.CountryId });

        modelBuilder.Entity<OGSquad>()
            .HasKey(s => new { s.TeamId, s.ParticipantId });

        modelBuilder.Entity<OGEvent>()
            .HasOne(e => e.Game)
            .WithMany(g => g.Events)
            .HasForeignKey(e => e.GameId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<OGEventVenue>()
            .HasOne(ev => ev.Event)
            .WithMany(e => e.EventVenues)
            .HasForeignKey(ev => ev.EventId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<OGEventVenue>()
            .HasOne(ev => ev.Venue)
            .WithMany(v => v.EventVenues)
            .HasForeignKey(ev => ev.VenueId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<OGAthleteCountry>()
            .HasOne(ac => ac.Athlete)
            .WithMany(a => a.AthleteCountries)
            .HasForeignKey(ac => ac.AthleteId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<OGAthleteCountry>()
            .HasOne(ac => ac.Country)
            .WithMany(c => c.AthleteCountries)
            .HasForeignKey(ac => ac.CountryId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<OGSquad>()
            .HasOne(s => s.Team)
            .WithMany(t => t.Squads)
            .HasForeignKey(s => s.TeamId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<OGSquad>()
            .HasOne(s => s.Participant)
            .WithMany(p => p.Squads)
            .HasForeignKey(s => s.ParticipantId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        base.OnModelCreating(modelBuilder);
    }
}