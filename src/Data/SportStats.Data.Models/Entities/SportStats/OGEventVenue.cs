namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations.Schema;

[Table("OlympicGames_EventsVenues", Schema = "dbo")]
public class OGEventVenue
{
    public int EventId { get; set; }
    public virtual OGEvent Event { get; set; }

    public int VenueId { get; set; }
    public virtual OGVenue Venue { get; set; }
}