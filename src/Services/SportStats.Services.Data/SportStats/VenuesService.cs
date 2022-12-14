namespace SportStats.Services.Data.SportStats;

using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Cache.OlympicGames;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;
using global::SportStats.Services.Mapper.Extensions;

using Microsoft.EntityFrameworkCore;

public class VenuesService : BaseSportStatsService, IVenuesService
{
    public VenuesService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public ICollection<VenueCacheModel> GetVenueCacheModels()
    {
        var venues = this.Context
            .OGVenues
            .AsNoTracking()
            .To<VenueCacheModel>()
            .ToList();

        return venues;
    }

    public async Task<OGVenue> GetVenueAsync(int number)
    {
        var venue = await this.Context
            .OGVenues
            .FirstOrDefaultAsync(v => v.Number == number);

        return venue;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        this.Context.Entry(entity).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return entity;
    }
}