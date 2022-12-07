namespace SportStats.Data.Models.Cache;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Mapper.Interfaces;

public class OGVenueCacheModel : IMapFrom<OGVenue>
{
    public int Id { get; set; }

    public int Number { get; set; }
}