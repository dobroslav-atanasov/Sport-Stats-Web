namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("OlympicGames_Venues", Schema = "dbo")]
public class OGVenue : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGVenue>
{
    [Required]
    public int Number { get; set; }

    [Required]
    [MaxLength(500)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; }

    [MaxLength(500)]
    public string EnglishName { get; set; }

    [MaxLength(1000)]
    public string FullName { get; set; }

    public double? LatitudeCoordinate { get; set; }

    public double? LongitudeCoordinate { get; set; }

    public int? Opened { get; set; }

    public int? Demolished { get; set; }

    public string Capacity { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGVenue other)
    {
        var isUpdated = false;

        if (this.Name != other.Name)
        {
            this.Name = other.Name;
            isUpdated = true;
        }

        if (this.City != other.City)
        {
            this.City = other.City;
            isUpdated = true;
        }

        if (this.EnglishName != other.EnglishName)
        {
            this.EnglishName = other.EnglishName;
            isUpdated = true;
        }

        if (this.FullName != other.FullName)
        {
            this.FullName = other.FullName;
            isUpdated = true;
        }

        if (this.LatitudeCoordinate != other.LatitudeCoordinate)
        {
            this.LatitudeCoordinate = other.LatitudeCoordinate;
            isUpdated = true;
        }

        if (this.LongitudeCoordinate != other.LongitudeCoordinate)
        {
            this.LongitudeCoordinate = other.LongitudeCoordinate;
            isUpdated = true;
        }

        if (this.Opened != other.Opened)
        {
            this.Opened = other.Opened;
            isUpdated = true;
        }

        if (this.Demolished != other.Demolished)
        {
            this.Demolished = other.Demolished;
            isUpdated = true;
        }

        if (this.Capacity != other.Capacity)
        {
            this.Capacity = other.Capacity;
            isUpdated = true;
        }

        return isUpdated;
    }
}