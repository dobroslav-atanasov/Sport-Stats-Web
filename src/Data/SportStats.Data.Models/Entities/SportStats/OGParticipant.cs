namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;
using global::SportStats.Data.Models.Enumerations;

[Table("OlympicGames_Participants", Schema = "dbo")]
public class OGParticipant : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGParticipant>
{
    public OGParticipant()
    {
        this.Squads = new HashSet<OGSquad>();
    }

    public int AthleteId { get; set; }
    public virtual OGAthlete Athlete { get; set; }

    public int EventId { get; set; }
    public virtual OGEvent Event { get; set; }

    public string Country { get; set; }

    public int? AgeYears { get; set; }

    public int? AgeDays { get; set; }

    public MedalType Medal { get; set; } = MedalType.None;

    [NotMapped]
    public int OlympediaNumber { get; set; }

    public bool IsCoach { get; set; } = false;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<OGSquad> Squads { get; set; }

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

        if (this.Medal != other.Medal)
        {
            this.Medal = other.Medal;
            isUpdated = true;
        }

        return isUpdated;
    }
}