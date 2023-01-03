namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("OlympicGames_Countries", Schema = "dbo")]
public class OGCountry : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGCountry>
{
    public OGCountry()
    {
        this.AthleteCountries = new HashSet<OGAthleteCountry>();
        this.Games = new HashSet<OGGame>();
        this.Teams = new HashSet<OGTeam>();
    }

    [Required]
    [MaxLength(50)]
    public string CountryName { get; set; }

    [Required]
    [StringLength(3)]
    public string Code { get; set; }

    [MaxLength(500)]
    public string CommitteeName { get; set; }

    [MaxLength(50)]
    public string CommitteeAbbreavition { get; set; }

    public int? CommitteeFoundedYear { get; set; }

    public int? CommitteeDisbandedYear { get; set; }

    public int? CommitteeRecognizedYear { get; set; }

    [StringLength(3)]
    public string RelatedCountry { get; set; }

    public byte[] CountryFlag { get; set; }

    public byte[] CommitteeFlag { get; set; }

    [MaxLength(10000)]
    public string CountryDescription { get; set; }

    [MaxLength(10000)]
    public string CommitteeDescription { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<OGAthleteCountry> AthleteCountries { get; set; }

    public virtual ICollection<OGGame> Games { get; set; }

    public virtual ICollection<OGTeam> Teams { get; set; }

    public bool Update(OGCountry other)
    {
        var isUpdated = false;

        if (this.CommitteeName != other.CommitteeName)
        {
            this.CommitteeName = other.CommitteeName;
            isUpdated = true;
        }

        if (this.CommitteeAbbreavition != other.CommitteeAbbreavition)
        {
            this.CommitteeAbbreavition = other.CommitteeAbbreavition;
            isUpdated = true;
        }

        if (this.CommitteeFoundedYear != other.CommitteeFoundedYear)
        {
            this.CommitteeFoundedYear = other.CommitteeFoundedYear;
            isUpdated = true;
        }

        if (this.CommitteeDisbandedYear != other.CommitteeDisbandedYear)
        {
            this.CommitteeDisbandedYear = other.CommitteeDisbandedYear;
            isUpdated = true;
        }

        if (this.CommitteeRecognizedYear != other.CommitteeRecognizedYear)
        {
            this.CommitteeRecognizedYear = other.CommitteeRecognizedYear;
            isUpdated = true;
        }

        if (this.RelatedCountry != other.RelatedCountry)
        {
            this.RelatedCountry = other.RelatedCountry;
            isUpdated = true;
        }

        if (this.CountryFlag != other.CountryFlag)
        {
            this.CountryFlag = other.CountryFlag;
            isUpdated = true;
        }

        if (this.CommitteeFlag != other.CommitteeFlag)
        {
            this.CommitteeFlag = other.CommitteeFlag;
            isUpdated = true;
        }

        if (this.CountryDescription != other.CountryDescription)
        {
            this.CountryDescription = other.CountryDescription;
            isUpdated = true;
        }

        if (this.CommitteeDescription != other.CommitteeDescription)
        {
            this.CommitteeDescription = other.CommitteeDescription;
            isUpdated = true;
        }

        return isUpdated;
    }
}