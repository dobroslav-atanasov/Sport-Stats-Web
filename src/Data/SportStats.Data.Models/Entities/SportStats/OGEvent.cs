namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("OlympicGames_Events", Schema = "dbo")]
public class OGEvent : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGEvent>
{
    [Required]
    [MaxLength(200)]
    public string OriginalName { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string NormalizedName { get; set; }

    public int DisciplineId { get; set; }
    public virtual OGDiscipline Discipline { get; set; }

    public int GameId { get; set; }
    public virtual OGGame Game { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? EndDate { get; set; }

    [Required]
    public bool IsTeamEvent { get; set; } = false;

    [MaxLength(200)]
    public string AdditionalInfo { get; set; }

    public int Athletes { get; set; }

    public int Countries { get; set; }

    public string Format { get; set; }

    public string Description { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGEvent other)
    {
        var isUpdated = false;

        if (this.Name != other.Name)
        {
            this.Name = other.Name;
            isUpdated = true;
        }

        if (this.NormalizedName != other.NormalizedName)
        {
            this.NormalizedName = other.NormalizedName;
            isUpdated = true;
        }

        if (this.StartDate != other.StartDate)
        {
            this.StartDate = other.StartDate;
            isUpdated = true;
        }

        if (this.EndDate != other.EndDate)
        {
            this.EndDate = other.EndDate;
            isUpdated = true;
        }

        if (this.IsTeamEvent != other.IsTeamEvent)
        {
            this.IsTeamEvent = other.IsTeamEvent;
            isUpdated = true;
        }

        if (this.AdditionalInfo != other.AdditionalInfo)
        {
            this.AdditionalInfo = other.AdditionalInfo;
            isUpdated = true;
        }

        if (this.Athletes != other.Athletes)
        {
            this.Athletes = other.Athletes;
            isUpdated = true;
        }

        if (this.Countries != other.Countries)
        {
            this.Countries = other.Countries;
            isUpdated = true;
        }

        if (this.Format != other.Format)
        {
            this.Format = other.Format;
            isUpdated = true;
        }

        if (this.Description != other.Description)
        {
            this.Description = other.Description;
            isUpdated = true;
        }

        return isUpdated;
    }
}