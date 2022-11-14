namespace SportStats.Services.Data.SportStats;

using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;
using global::SportStats.Services.Mapper.Extensions;

using Microsoft.EntityFrameworkCore;

public class CountriesService : BaseSportStatsService, ICountriesService
{
    public CountriesService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TCountry> AddAsync<TCountry>(TCountry country)
    {
        await this.Context.AddAsync(country);
        await this.Context.SaveChangesAsync();

        return country;
    }

    public async Task<WorldCountry> GetWorldCountryAsync(string code)
    {
        var country = await this.Context
            .Countries
            .FirstOrDefaultAsync(c => c.Code == code);

        return country;
    }

    public async Task<OGCountry> GetOlympicGameCountryAsync(string code)
    {
        var country = await this.Context
            .OGCountries
            .FirstOrDefaultAsync(c => c.Code == code);

        return country;
    }

    public async Task<TCountry> UpdateAsync<TCountry>(TCountry country)
    {
        this.Context.Entry(country).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return country;
    }

    public ICollection<OGCountryCacheModel> GetOGCountriesCache()
    {
        var countries = this.Context
            .OGCountries
            .AsNoTracking()
            .To<OGCountryCacheModel>()
            .ToList();

        return countries;
    }
}