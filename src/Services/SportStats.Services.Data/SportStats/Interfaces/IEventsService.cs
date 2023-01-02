namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache.OlympicGames;
using global::SportStats.Data.Models.Entities.SportStats;

public interface IEventsService : IAddable, IUpdatable
{
    Task<OGEvent> GetEventAsync(string name, int disciplineId, int gameId);

    ICollection<EventCacheModel> GetEventCacheModels();
}