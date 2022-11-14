namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Data.Models.Enumerations;
using global::SportStats.Services.Data.SportStats.Interfaces;

using Microsoft.EntityFrameworkCore;

public class GamesService : BaseSportStatsService, IGamesService
{
    public GamesService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<OGGame> AddAsync(OGGame game)
    {
        await this.Context.AddAsync(game);
        await this.Context.SaveChangesAsync();

        return game;
    }

    public async Task<OGGame> GetGameAsync(int year, OlympicGameType type)
    {
        var game = await this.Context
            .OGGames
            .FirstOrDefaultAsync(g => g.Year == year && g.Type == type);

        return game;
    }

    public async Task<OGGame> UpdateAsync(OGGame game)
    {
        this.Context.Entry(game).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return game;
    }
}