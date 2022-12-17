namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class ParticipantsService : BaseSportStatsService, IParticipantsService
{
    public ParticipantsService(SportStatsDbContext context)
        : base(context)
    {
    }

    public Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<OGParticipant> GetParticipantAsync(int athleteId, int eventId)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }
}