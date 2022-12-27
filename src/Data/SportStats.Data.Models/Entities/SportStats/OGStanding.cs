namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;
using global::SportStats.Data.Models.Enumerations;

[Table("OlympicGames_Standings", Schema = "dbo")]
public class OGStanding : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGStanding>
{
    [Required]
    public int EventId { get; set; }
    public virtual OGEvent Event { get; set; }

    [Required]
    public bool IsTeamEvent { get; set; } = false;

    public int? ParticipantId { get; set; }
    public virtual OGParticipant Participant { get; set; }

    public int? TeamId { get; set; }
    public virtual OGTeam Team { get; set; }

    public int? Rank { get; set; }

    [Required]
    public MedalType Medal { get; set; }

    [Required]
    public FinishStatus FinishStatus { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGStanding other)
    {
        var isUpdated = false;

        if (this.Rank != other.Rank)
        {
            this.Rank = other.Rank;
            isUpdated = true;
        }

        if (this.Medal != other.Medal)
        {
            this.Medal = other.Medal;
            isUpdated = true;
        }

        if (this.FinishStatus != other.FinishStatus)
        {
            this.FinishStatus = other.FinishStatus;
            isUpdated = true;
        }

        return isUpdated;
    }
}