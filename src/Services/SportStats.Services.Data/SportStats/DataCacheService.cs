namespace SportStats.Services.Data.SportStats;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class DataCacheService : IDataCacheService
{
    private readonly Lazy<ICollection<OGCountryCacheModel>> ogCountryCacheModels;
    private readonly Lazy<ICollection<OGDisciplineCacheModel>> ogDisciplineCacheModels;
    private readonly Lazy<ICollection<OGGameCacheModel>> ogGameCacheModels;
    private readonly ICountriesService countriesService;
    private readonly IDisciplinesService disciplinesService;
    private readonly IGamesService gamesService;

    public DataCacheService(ICountriesService countriesService, IDisciplinesService disciplinesService, IGamesService gamesService)
    {
        this.countriesService = countriesService;
        this.disciplinesService = disciplinesService;
        this.gamesService = gamesService;
        this.ogCountryCacheModels = new Lazy<ICollection<OGCountryCacheModel>>(() => this.countriesService.GetOGCountriesCache());
        this.ogDisciplineCacheModels = new Lazy<ICollection<OGDisciplineCacheModel>>(() => this.disciplinesService.GetOGDisciplinesCache());
        this.ogGameCacheModels = new Lazy<ICollection<OGGameCacheModel>>(() => this.gamesService.GetOGGamesCache());
    }

    public ICollection<OGCountryCacheModel> OGCountriesCache => this.ogCountryCacheModels.Value;

    public ICollection<OGDisciplineCacheModel> OGDisciplinesCache => this.ogDisciplineCacheModels.Value;

    public ICollection<OGGameCacheModel> OGGamesCache => this.ogGameCacheModels.Value;
}