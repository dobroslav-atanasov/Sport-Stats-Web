namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Entities.SportStats;

public interface ICountryService
{
    Task<Country> AddAsync(Country country);

    Task<Country> UpdateAsync(Country country);
}