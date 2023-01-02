namespace SportStats.Data.Models.Cache.OlympicGames;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Mapper.Interfaces;

public class EventCacheModel : IMapFrom<OGEvent>
{
    public int Id { get; set; }

    public string OriginalName { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public bool IsTeamEvent { get; set; }

    public int DisciplineId { get; set; }

    public int GameId { get; set; }
}