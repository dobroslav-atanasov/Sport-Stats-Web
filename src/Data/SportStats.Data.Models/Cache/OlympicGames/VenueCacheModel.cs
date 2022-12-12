namespace SportStats.Data.Models.Cache.OlympicGames;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Mapper.Interfaces;

public class VenueCacheModel : IMapFrom<OGVenue>
{
    public int Id { get; set; }

    public int Number { get; set; }
}