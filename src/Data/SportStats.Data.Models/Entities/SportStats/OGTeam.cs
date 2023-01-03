namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;
using global::SportStats.Data.Models.Enumerations;

[Table("OlympicGames_Teams", Schema = "dbo")]
public class OGTeam : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGTeam>
{
    public OGTeam()
    {
        this.Squads = new HashSet<OGSquad>();
    }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public int EventId { get; set; }
    public virtual OGEvent Event { get; set; }

    public int CountryId { get; set; }
    public virtual OGCountry Country { get; set; }

    public string CountryName { get; set; }

    public int? CoachId { get; set; }
    public virtual OGAthlete Coach { get; set; }

    public MedalType Medal { get; set; } = MedalType.None;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<OGSquad> Squads { get; set; }

    public bool Update(OGTeam other)
    {
        var isUpdated = false;

        if (this.CoachId != other.CoachId)
        {
            this.CoachId = other.CoachId;
            isUpdated = true;
        }

        if (this.CountryName != other.CountryName)
        {
            this.CountryName = other.CountryName;
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