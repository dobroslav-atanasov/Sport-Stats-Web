namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("OlympicGames_Disciplines", Schema = "dbo")]
public class OGDiscipline : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGDiscipline>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(3)]
    public string Abbreviation { get; set; }

    public int SportId { get; set; }
    public virtual OGSport Sport { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGDiscipline other)
    {
        var isUpdated = false;

        if (this.Abbreviation != other.Abbreviation)
        {
            this.Abbreviation = other.Abbreviation;
            isUpdated = true;
        }

        return isUpdated;
    }
}