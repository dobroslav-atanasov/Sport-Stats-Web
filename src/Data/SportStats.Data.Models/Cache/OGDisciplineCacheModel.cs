namespace SportStats.Data.Models.Cache;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Mapper.Interfaces;

public class OGDisciplineCacheModel : IMapFrom<OGDiscipline>
{
    public int Id { get; set; }

    public string Name { get; set; }
}