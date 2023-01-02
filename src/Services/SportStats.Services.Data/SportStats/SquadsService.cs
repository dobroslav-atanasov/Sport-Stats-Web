namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Factory.Interfaces;
using global::SportStats.Services.Data.SportStats.Interfaces;

using Microsoft.EntityFrameworkCore;

public class SquadsService : BaseSportStatsService, ISquadsService
{
    private readonly IDbContextFactory dbContextFactory;

    public SquadsService(SportStatsDbContext context, IDbContextFactory dbContextFactory)
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

    public bool SquadExists(int participantId, int teamId)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        return context.OGSquads.Any(x => x.ParticipantId == participantId && x.TeamId == teamId);
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return entity;
    }
}