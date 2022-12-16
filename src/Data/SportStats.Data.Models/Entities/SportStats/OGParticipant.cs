namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("OlympicGames_Participants", Schema = "dbo")]
public class OGParticipant : BaseEntity<int>, IUpdatable<OGParticipant>
{
    public int AthleteId { get; set; }
    public virtual OGAthlete Athlete { get; set; }

    public int EventId { get; set; }
    public virtual OGEvent Event { get; set; }

    public string Country { get; set; }

    public int? AgeYears { get; set; }

    public int? AgeDays { get; set; }

    [NotMapped]
    public int OlympediaNumber { get; set; }

    public bool Update(OGParticipant other)
    {
        var isUpdated = false;

        if (this.Country != other.Country)
        {
            this.Country = other.Country;
            isUpdated = true;
        }

        if (this.AgeYears != other.AgeYears)
        {
            this.AgeYears = other.AgeYears;
            isUpdated = true;
        }

        if (this.AgeDays != other.AgeDays)
        {
            this.AgeDays = other.AgeDays;
            isUpdated = true;
        }

        return isUpdated;
    }
}