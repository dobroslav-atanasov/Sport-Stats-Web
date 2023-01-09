namespace SportStats.Common.Converters.Olympedia;

using Microsoft.Extensions.Logging;

using SportStats.Services.Data.SportStats.Interfaces;

public class AdditionalConverter
{
    private readonly ILogger<AdditionalConverter> logger;
    private readonly IGamesService gamesService;
    private readonly IDataCacheService dataCacheService;

    public AdditionalConverter(ILogger<AdditionalConverter> logger, IGamesService gamesService, IDataCacheService dataCacheService)
    {
        this.logger = logger;
        this.gamesService = gamesService;
        this.dataCacheService = dataCacheService;
    }

    public async Task ProcessGamesAsync()
    {
        var games = this.gamesService.GetGames().ToList();

        foreach (var game in games)
        {
            var asd = await this.gamesService.GetGameAsync(game.Year, game.Type);
            var adsas = asd.Events.ToList();
            ;
        }
        ;
    }
}