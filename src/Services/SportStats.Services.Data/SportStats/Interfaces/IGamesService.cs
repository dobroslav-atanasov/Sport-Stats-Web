﻿namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Data.Models.Enumerations;

public interface IGamesService : IAddable, IUpdatable
{
    Task<OGGame> GetGameAsync(int year, OlympicGameType type);

    ICollection<OGGameCacheModel> GetOGGamesCache();
}