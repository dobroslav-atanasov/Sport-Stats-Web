namespace SportStats.Services.Data.SportStats;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class DataCacheService : IDataCacheService
{
    private readonly Lazy<ICollection<OGCountryCacheModel>> ogCountryCacheModels;
    private readonly Lazy<ICollection<OGDisciplineCacheModel>> ogDisciplineCacheModels;
    private readonly Lazy<ICollection<OGGameCacheModel>> ogGameCacheModels;
    private readonly Lazy<ICollection<OGVenueCacheModel>> ogVenueCacheModels;
    private readonly ICountriesService countriesService;
    private readonly IDisciplinesService disciplinesService;
    private readonly IGamesService gamesService;
    private readonly IVenuesService venuesService;

    public DataCacheService(ICountriesService countriesService, IDisciplinesService disciplinesService, IGamesService gamesService, IVenuesService venuesService)
    {
        this.countriesService = countriesService;
        this.disciplinesService = disciplinesService;
        this.gamesService = gamesService;
        this.venuesService = venuesService;
        this.ogCountryCacheModels = new Lazy<ICollection<OGCountryCacheModel>>(() => this.countriesService.GetOGCountriesCache());
        this.ogDisciplineCacheModels = new Lazy<ICollection<OGDisciplineCacheModel>>(() => this.disciplinesService.GetOGDisciplinesCache());
        this.ogGameCacheModels = new Lazy<ICollection<OGGameCacheModel>>(() => this.gamesService.GetOGGamesCache());
        this.ogVenueCacheModels = new Lazy<ICollection<OGVenueCacheModel>>(() => this.venuesService.GetOGVenuesCache());
    }

    public ICollection<OGCountryCacheModel> OGCountriesCache => this.ogCountryCacheModels.Value;

    public ICollection<OGDisciplineCacheModel> OGDisciplinesCache => this.ogDisciplineCacheModels.Value;

    public ICollection<OGGameCacheModel> OGGamesCache => this.ogGameCacheModels.Value;

    public ICollection<OGVenueCacheModel> OGVenuesCache => this.ogVenueCacheModels.Value;
}