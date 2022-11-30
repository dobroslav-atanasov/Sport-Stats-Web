namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;

public interface IDataCacheService
{
    ICollection<OGCountryCacheModel> OGCountriesCache { get; }

    ICollection<OGDisciplineCacheModel> OGDisciplinesCache { get; }

    ICollection<OGGameCacheModel> OGGamesCache { get; }

    ICollection<OGVenueCacheModel> OGVenuesCache { get; }
}