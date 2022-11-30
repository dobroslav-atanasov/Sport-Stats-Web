namespace SportStats.Services.Data.SportStats.Interfaces;

public interface IEventVenueService : IAddable
{
    bool EventVenueExists(int eventId, int venueId);
}