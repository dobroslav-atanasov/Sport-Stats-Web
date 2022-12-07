namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Factory.Interfaces;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class AthleteCountryService : BaseSportStatsService, IAthleteCountryService
{
    private readonly IDbContextFactory dbContextFactory;

    public AthleteCountryService(SportStatsDbContext context, IDbContextFactory dbContextFactory)
        : base(context)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public bool AthleteCountryExists(int athleteId, int countryId)
    {
        using var context = this.dbContextFactory.CreateSportStatsDbContext();

        return context.OGAthleteCountries.Any(x => x.AthleteId == athleteId && x.CountryId == countryId);
    }
}