namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Entities.SportStats;

public interface ISportsService : IAddable, IUpdatable
{
    Task<OGSport> GetSportAsync(string name);
}