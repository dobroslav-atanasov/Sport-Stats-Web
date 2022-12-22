namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Entities.SportStats;

public interface ITeamsService : IAddable, IUpdatable
{
    Task<OGTeam> GetTeamAsync(string name, int eventId, int countryId);
}