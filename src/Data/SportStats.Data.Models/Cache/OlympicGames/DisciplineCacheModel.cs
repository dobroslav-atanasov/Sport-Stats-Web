namespace SportStats.Data.Models.Cache.OlympicGames;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Mapper.Interfaces;

public class DisciplineCacheModel : IMapFrom<OGDiscipline>
{
    public int Id { get; set; }

    public string Name { get; set; }
}