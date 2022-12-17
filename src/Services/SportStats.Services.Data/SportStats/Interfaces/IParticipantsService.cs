namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Entities.SportStats;

public interface IParticipantsService : IAddable, IUpdatable
{
    Task<OGParticipant> GetParticipantAsync(int athleteId, int eventId);
}