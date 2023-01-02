namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Factory.Interfaces;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;

using Microsoft.EntityFrameworkCore;

public class ParticipantsService : BaseSportStatsService, IParticipantsService
{
    private readonly IDbContextFactory dbContextFactory;

    public ParticipantsService(SportStatsDbContext context, IDbContextFactory dbContextFactory)
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

    public async Task<OGParticipant> GetParticipantAsync(int athleteId, int eventId)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        var participant = await context
            .OGParticipants
            .FirstOrDefaultAsync(p => p.AthleteId == athleteId && p.EventId == eventId);

        return participant;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return entity;
    }
}