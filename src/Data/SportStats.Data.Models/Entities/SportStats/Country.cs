namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("Countries", Schema = "dbo")]
public class Country : BaseEntity<int>, ICreatableEntity, IDeletableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    [Required]
    public bool IsIndependent { get; set; } = false;

    [Required]
    [StringLength(2)]
    public string Alpha2Code { get; set; }

    [Required]
    [StringLength(3)]
    public string Alpha3Code { get; set; }

    [MaxLength(100)]
    public string Capital { get; set; }

    [MaxLength(50)]
    public string Continent { get; set; }

    [MaxLength(200)]
    public string MemberOf { get; set; }

    [Required]
    public int Population { get; set; }

    [Required]
    public int TotalArea { get; set; }

    [MaxLength(50)]
    public string HighestPointName { get; set; }

    public int? HighestPoint { get; set; }

    [MaxLength(50)]
    public string LowestPointName { get; set; }

    public int? LowestPoint { get; set; }

    public byte[] Flag { get; set; }

    public DateTime CreatedOn => DateTime.UtcNow;

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedOn { get; set; }
}