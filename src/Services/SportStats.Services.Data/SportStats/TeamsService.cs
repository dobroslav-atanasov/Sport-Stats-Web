namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Factory.Interfaces;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;

using Microsoft.EntityFrameworkCore;

public class TeamsService : BaseSportStatsService, ITeamsService
{
    private readonly IDbContextFactory dbContextFactory;

    public TeamsService(SportStatsDbContext context, IDbContextFactory dbContextFactory)
        : base(context)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<OGTeam> GetTeamAsync(string name, int eventId, int countryId)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        var team = await context
            .OGTeams
            .FirstOrDefaultAsync(t => t.Name == name && t.EventId == eventId && t.CountryId == countryId);

        return team;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return entity;
    }
}