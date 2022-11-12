namespace SportStats.Data.Models.Entities.SportStats;

using System;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportStats.Data.Models.Entities.Interfaces;

[Table("OlympicGames_Games", Schema = "dbo")]
public class OGGame : BaseEntity<int>, ICreatableEntity, IDeletableEntity, IUpdatable<OGGame>
{
    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool Update(OGGame other)
    {
        throw new NotImplementedException();
    }
}