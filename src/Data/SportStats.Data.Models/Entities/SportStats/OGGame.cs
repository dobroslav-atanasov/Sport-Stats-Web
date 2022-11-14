namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;
using global::SportStats.Data.Models.Enumerations;

[Table("OlympicGames_Games", Schema = "dbo")]
public class OGGame : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGGame>
{
    [Required]
    public int Year { get; set; }

    [MaxLength(10)]
    public string Number { get; set; }

    [Required]
    [MaxLength(100)]
    public string HostCity { get; set; }

    [Required]
    public int HostCountryId { get; set; }
    public virtual OGCountry HostCountry { get; set; }

    [Required]
    public OlympicGameType Type { get; set; }

    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? OpenDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? CloseDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? StartCompetitionDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? EndCompetitionDate { get; set; }

    public int ParticipantAthletes { get; set; }

    public int ParticipantCountries { get; set; }

    public int MedalEvents { get; set; }

    public int MedalDisciplines { get; set; }

    [MaxLength(500)]
    public string OpenBy { get; set; }

    [MaxLength(5000)]
    public string Torchbearers { get; set; }

    [MaxLength(500)]
    public string AthleteOathBy { get; set; }

    [MaxLength(500)]
    public string JudgeOathBy { get; set; }

    [MaxLength(500)]
    public string CoachOathBy { get; set; }

    [MaxLength(500)]
    public string OlympicFlagBearers { get; set; }

    public string Description { get; set; }

    public string BidProcess { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGGame other)
    {
        var isUpdated = false;

        if (this.Number != other.Number)
        {
            this.Number = other.Number;
            isUpdated = true;
        }

        if (this.HostCity != other.HostCity)
        {
            this.HostCity = other.HostCity;
            isUpdated = true;
        }

        if (this.HostCountryId != other.HostCountryId)
        {
            this.HostCountryId = other.HostCountryId;
            isUpdated = true;
        }

        if (this.OfficialName != other.OfficialName)
        {
            this.OfficialName = other.OfficialName;
            isUpdated = true;
        }

        if (this.OpenDate != other.OpenDate)
        {
            this.OpenDate = other.OpenDate;
            isUpdated = true;
        }

        if (this.CloseDate != other.CloseDate)
        {
            this.CloseDate = other.CloseDate;
            isUpdated = true;
        }

        if (this.StartCompetitionDate != other.StartCompetitionDate)
        {
            this.StartCompetitionDate = other.StartCompetitionDate;
            isUpdated = true;
        }

        if (this.EndCompetitionDate != other.EndCompetitionDate)
        {
            this.EndCompetitionDate = other.EndCompetitionDate;
            isUpdated = true;
        }

        if (this.ParticipantAthletes != other.ParticipantAthletes)
        {
            this.ParticipantAthletes = other.ParticipantAthletes;
            isUpdated = true;
        }

        if (this.ParticipantCountries != other.ParticipantCountries)
        {
            this.ParticipantCountries = other.ParticipantCountries;
            isUpdated = true;
        }

        if (this.MedalEvents != other.MedalEvents)
        {
            this.MedalEvents = other.MedalEvents;
            isUpdated = true;
        }

        if (this.MedalDisciplines != other.MedalDisciplines)
        {
            this.MedalDisciplines = other.MedalDisciplines;
            isUpdated = true;
        }

        if (this.OpenBy != other.OpenBy)
        {
            this.OpenBy = other.OpenBy;
            isUpdated = true;
        }

        if (this.Torchbearers != other.Torchbearers)
        {
            this.Torchbearers = other.Torchbearers;
            isUpdated = true;
        }

        if (this.AthleteOathBy != other.AthleteOathBy)
        {
            this.AthleteOathBy = other.AthleteOathBy;
            isUpdated = true;
        }

        if (this.JudgeOathBy != other.JudgeOathBy)
        {
            this.JudgeOathBy = other.JudgeOathBy;
            isUpdated = true;
        }

        if (this.CoachOathBy != other.CoachOathBy)
        {
            this.CoachOathBy = other.CoachOathBy;
            isUpdated = true;
        }

        if (this.OlympicFlagBearers != other.OlympicFlagBearers)
        {
            this.OlympicFlagBearers = other.OlympicFlagBearers;
            isUpdated = true;
        }

        if (this.Description != other.Description)
        {
            this.Description = other.Description;
            isUpdated = true;
        }

        if (this.BidProcess != other.BidProcess)
        {
            this.BidProcess = other.BidProcess;
            isUpdated = true;
        }

        return isUpdated;
    }
}