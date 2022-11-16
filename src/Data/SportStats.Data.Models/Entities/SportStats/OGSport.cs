namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;
using global::SportStats.Data.Models.Enumerations;

[Table("OlympicGames_Sports", Schema = "dbo")]
public class OGSport : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGSport>
{
    public OGSport()
    {
        this.Disciplines = new HashSet<OGDiscipline>();
    }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(3)]
    public string Abbreviation { get; set; }

    [Required]
    public OlympicGameType Type { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<OGDiscipline> Disciplines { get; set; }

    public bool Update(OGSport other)
    {
        var isUpdated = false;

        if (this.Abbreviation != other.Abbreviation)
        {
            this.Abbreviation = other.Abbreviation;
            isUpdated = true;
        }

        if (this.Type != other.Type)
        {
            this.Type = other.Type;
            isUpdated = true;
        }

        return isUpdated;
    }
}