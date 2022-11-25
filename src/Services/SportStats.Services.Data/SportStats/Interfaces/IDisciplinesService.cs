namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;

public interface IDisciplinesService : IAddable, IUpdatable
{
    Task<OGDiscipline> GetDisciplineAsync(string name);

    ICollection<OGDisciplineCacheModel> GetOGDisciplinesCache();
}