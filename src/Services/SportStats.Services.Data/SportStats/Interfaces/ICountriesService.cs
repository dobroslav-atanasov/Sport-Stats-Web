namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;

public interface ICountriesService : IAddable, IUpdatable
{
    Task<WorldCountry> GetWorldCountryAsync(string code);

    Task<OGCountry> GetOlympicGameCountryAsync(string code);

    ICollection<OGCountryCacheModel> GetOGCountriesCache();
}