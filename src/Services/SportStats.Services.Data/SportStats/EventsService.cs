namespace SportStats.Services.Data.SportStats;

using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Cache.OlympicGames;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;
using global::SportStats.Services.Mapper.Extensions;

using Microsoft.EntityFrameworkCore;

public class EventsService : BaseSportStatsService, IEventsService
{
    public EventsService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public async Task<OGEvent> GetEventAsync(string name, int disciplineId, int gameId)
    {
        var @event = await this.Context
            .OGEvents
            .FirstOrDefaultAsync(e => e.OriginalName == name && e.DisciplineId == disciplineId && e.GameId == gameId);

        return @event;
    }

    public ICollection<EventCacheModel> GetEventCacheModels()
    {
        var events = this.Context
            .OGEvents
            .AsNoTracking()
            .To<EventCacheModel>()
            .ToList();

        return events;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        this.Context.Entry(entity).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return entity;
    }
}