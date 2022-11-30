namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class EventVenueService : BaseSportStatsService, IEventVenueService
{
    public EventVenueService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public bool EventVenueExists(int eventId, int venueId)
    {
        return this.Context.OGEventVenues.Any(x => x.EventId == eventId && x.VenueId == venueId);
    }
}