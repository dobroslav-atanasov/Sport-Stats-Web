namespace SportStats.Data.Models.Cache.OlympicGames;

using AutoMapper;

using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Mapper.Interfaces;

public class CountryCacheModel : IMapFrom<OGCountry>, ICustomMap
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public void CreateMap(IProfileExpression profileExpression)
    {
        profileExpression.CreateMap<OGCountry, CountryCacheModel>()
            .ForMember(x => x.Name, options => options.MapFrom(x => x.CountryName));
    }
}