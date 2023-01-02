namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache.OlympicGames;

public interface IDataCacheService
{
    ICollection<CountryCacheModel> CountryCacheModels { get; }

    ICollection<DisciplineCacheModel> DisciplineCacheModels { get; }

    ICollection<GameCacheModel> GameCacheModels { get; }

    ICollection<VenueCacheModel> VenueCacheModels { get; }

    ICollection<EventCacheModel> EventCacheModels { get; }
}