namespace SportStats.Data.Models.Entities.Countries;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Countries")]
public class Country
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string Alpha2Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string NameUpperCase { get; set; }

    [MaxLength(50)]
    public string Continent { get; set; }

    [MaxLength(200)]
    public string Capital { get; set; }

    public string Flag4x3 { get; set; }

    public string Flag1x1 { get; set; }
}