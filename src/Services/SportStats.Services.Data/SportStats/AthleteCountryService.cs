namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class AthleteCountryService : BaseSportStatsService, IAthleteCountryService
{
    public AthleteCountryService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public bool AthleteCountryExists(int athleteId, int countryId)
    {
        return this.Context.OGAthleteCountries.Any(x => x.AthleteId == athleteId && x.CountryId == countryId);
    }
}