namespace SportStats.Services.Data.SportStats;

using global::SportStats.Data.Models.Cache.OlympicGames;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class DataCacheService : IDataCacheService
{
    private readonly Lazy<ICollection<CountryCacheModel>> countryCacheModels;
    private readonly Lazy<ICollection<DisciplineCacheModel>> diciplineCacheModels;
    private readonly Lazy<ICollection<GameCacheModel>> gameCacheModels;
    private readonly Lazy<ICollection<VenueCacheModel>> venueCacheModels;
    private readonly Lazy<ICollection<EventCacheModel>> eventCacheModels;
    private readonly ICountriesService countriesService;
    private readonly IDisciplinesService disciplinesService;
    private readonly IGamesService gamesService;
    private readonly IVenuesService venuesService;
    private readonly IEventsService eventsService;

    public DataCacheService(ICountriesService countriesService, IDisciplinesService disciplinesService, IGamesService gamesService, IVenuesService venuesService,
        IEventsService eventsService)
    {
        this.countriesService = countriesService;
        this.disciplinesService = disciplinesService;
        this.gamesService = gamesService;
        this.venuesService = venuesService;
        this.eventsService = eventsService;
        this.countryCacheModels = new Lazy<ICollection<CountryCacheModel>>(() => this.countriesService.GetCountryCacheModels());
        this.diciplineCacheModels = new Lazy<ICollection<DisciplineCacheModel>>(() => this.disciplinesService.GetDisciplineCacheModels());
        this.gameCacheModels = new Lazy<ICollection<GameCacheModel>>(() => this.gamesService.GetGameCacheModels());
        this.venueCacheModels = new Lazy<ICollection<VenueCacheModel>>(() => this.venuesService.GetVenueCacheModels());
        this.eventCacheModels = new Lazy<ICollection<EventCacheModel>>(() => this.eventsService.GetEventCacheModels());
    }

    public ICollection<CountryCacheModel> CountryCacheModels => this.countryCacheModels.Value;

    public ICollection<DisciplineCacheModel> DisciplineCacheModels => this.diciplineCacheModels.Value;

    public ICollection<GameCacheModel> GameCacheModels => this.gameCacheModels.Value;

    public ICollection<VenueCacheModel> VenueCacheModels => this.venueCacheModels.Value;

    public ICollection<EventCacheModel> EventCacheModels => this.eventCacheModels.Value;
}