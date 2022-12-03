namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations.Schema;

[Table("OlympicGames_AthletesCountries", Schema = "dbo")]
public class OGAthleteCountry
{
    public int AthleteId { get; set; }
    public virtual OGAthlete Athlete { get; set; }

    public int CountryId { get; set; }
    public virtual OGCountry Country { get; set; }
}