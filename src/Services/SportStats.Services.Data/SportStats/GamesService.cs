namespace SportStats.Services.Data.SportStats;

using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Data.Models.Enumerations;
using global::SportStats.Services.Data.SportStats.Interfaces;
using global::SportStats.Services.Mapper.Extensions;

using Microsoft.EntityFrameworkCore;

public class GamesService : BaseSportStatsService, IGamesService
{
    public GamesService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public async Task<OGGame> GetGameAsync(int year, OlympicGameType type)
    {
        var game = await this.Context
            .OGGames
            .FirstOrDefaultAsync(g => g.Year == year && g.Type == type);

        return game;
    }

    public ICollection<OGGameCacheModel> GetOGGamesCache()
    {
        var games = this.Context
            .OGGames
            .AsNoTracking()
            .To<OGGameCacheModel>()
            .ToList();

        return games;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        this.Context.Entry(entity).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return entity;
    }
}