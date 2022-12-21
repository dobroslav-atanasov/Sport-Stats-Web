namespace SportStats.Services.Data.SportStats.Interfaces;

public interface ISquadsService : IAddable, IUpdatable
{
    bool SquadExists(int participantId, int teamId);
}