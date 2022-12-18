namespace SportStats.Data.Models.Entities.SportStats;

using System.ComponentModel.DataAnnotations.Schema;

[Table("OlympicGames_Squads", Schema = "dbo")]
public class OGSquad
{
    public int TeamId { get; set; }
    public virtual OGTeam Team { get; set; }

    public int ParticipantId { get; set; }
    public virtual OGParticipant Participant { get; set; }
}