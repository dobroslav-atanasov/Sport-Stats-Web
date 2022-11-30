namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;

public interface IVenuesService : IAddable, IUpdatable
{
    Task<OGVenue> GetVenueAsync(int number);

    ICollection<OGVenueCacheModel> GetOGVenuesCache();
}