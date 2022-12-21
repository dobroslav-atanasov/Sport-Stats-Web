namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class SquadsService : BaseSportStatsService, ISquadsService
{
    public SquadsService(SportStatsDbContext context)
        : base(context)
    {
    }

    public Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public bool SquadExists(int participantId, int teamId)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        throw new NotImplementedException();
    }
}