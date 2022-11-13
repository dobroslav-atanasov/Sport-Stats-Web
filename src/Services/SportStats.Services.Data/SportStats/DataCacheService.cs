namespace SportStats.Services.Data.SportStats;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Services.Data.SportStats.Interfaces;

public class DataCacheService : IDataCacheService
{
    private readonly Lazy<ICollection<OGCountryCacheModel>> ogCountryCacheModels;
    private readonly ICountriesService countriesService;

    public DataCacheService(ICountriesService countriesService)
    {
        this.countriesService = countriesService;
        this.ogCountryCacheModels = new Lazy<ICollection<OGCountryCacheModel>>(() => this.countriesService.GetOGCountriesCache());
    }

    public ICollection<OGCountryCacheModel> OGCacheCountries => this.ogCountryCacheModels.Value;
}