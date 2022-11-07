namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;

using Microsoft.EntityFrameworkCore;

public class CountryService : BaseSportStatsService, ICountryService
{
    public CountryService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<Country> AddAsync(Country country)
    {
        await this.Context.AddAsync(country);
        await this.Context.SaveChangesAsync();

        return country;
    }

    public async Task<Country> UpdateAsync(Country country)
    {
        this.Context.Entry(country).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return country;
    }
}