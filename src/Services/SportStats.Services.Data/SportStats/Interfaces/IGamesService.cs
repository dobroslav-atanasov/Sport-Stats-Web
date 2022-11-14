namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Data.Models.Enumerations;

public interface IGamesService
{
    Task<OGGame> GetGameAsync(int year, OlympicGameType type);

    Task<OGGame> AddAsync(OGGame game);

    Task<OGGame> UpdateAsync(OGGame game);
}