﻿namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;
using global::SportStats.Data.Models.Enumerations;

[Table("OlympicGames_Athletes", Schema = "dbo")]
public class OGAthlete : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGAthlete>
{
    [Required]
    [Column(TypeName = "UNIQUEIDENTIFIER")]
    public Guid Identifier { get; set; }

    [Required]
    public int Number { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string EnglishName { get; set; }

    [MaxLength(200)]
    public string FullName { get; set; }

    public GenderType Gender { get; set; }

    public AthleteType Type { get; set; }

    [MaxLength(100)]
    public string Nationality { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? BirthDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? DiedDate { get; set; }

    [MaxLength(100)]
    public string BirthPlace { get; set; }

    [MaxLength(100)]
    public string DiedPlace { get; set; }

    public int? Height { get; set; }

    public int? Weight { get; set; }

    [MaxLength(200)]
    public string Association { get; set; }

    public string Description { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGAthlete other)
    {
        var isUpdated = false;

        if (this.Name != other.Name)
        {
            this.Name = other.Name;
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

        if (this.Gender != other.Gender)
        {
            this.Gender = other.Gender;
            isUpdated = true;
        }

        if (this.Type != other.Type)
        {
            this.Type = other.Type;
            isUpdated = true;
        }

        if (this.Nationality != other.Nationality)
        {
            this.Nationality = other.Nationality;
            isUpdated = true;
        }

        if (this.BirthDate != other.BirthDate)
        {
            this.BirthDate = other.BirthDate;
            isUpdated = true;
        }

        if (this.DiedDate != other.DiedDate)
        {
            this.DiedDate = other.DiedDate;
            isUpdated = true;
        }

        if (this.BirthPlace != other.BirthPlace)
        {
            this.BirthPlace = other.BirthPlace;
            isUpdated = true;
        }

        if (this.DiedPlace != other.DiedPlace)
        {
            this.DiedPlace = other.DiedPlace;
            isUpdated = true;
        }

        if (this.Height != other.Height)
        {
            this.Height = other.Height;
            isUpdated = true;
        }

        if (this.Weight != other.Weight)
        {
            this.Weight = other.Weight;
            isUpdated = true;
        }

        if (this.Association != other.Association)
        {
            this.Association = other.Association;
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