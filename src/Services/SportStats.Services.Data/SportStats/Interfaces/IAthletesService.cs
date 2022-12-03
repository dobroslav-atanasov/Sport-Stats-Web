namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Entities.SportStats;

public interface IAthletesService : IAddable, IUpdatable
{
    Task<OGAthlete> GetAthleteByNumberAsync(int number);
}