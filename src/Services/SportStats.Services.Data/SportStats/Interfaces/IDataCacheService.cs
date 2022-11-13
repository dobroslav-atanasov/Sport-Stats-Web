namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;

public interface IDataCacheService
{
    ICollection<OGCountryCacheModel> OGCacheCountries { get; }
}