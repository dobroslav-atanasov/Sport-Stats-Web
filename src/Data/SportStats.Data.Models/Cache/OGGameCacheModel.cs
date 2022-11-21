namespace SportStats.Data.Models.Cache;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Mapper.Interfaces;

public class OGGameCacheModel : IMapFrom<OGGame>
{
    public int Id { get; set; }

    public int Year { get; set; }

    public OlympicGameType Type { get; set; }
}