namespace SportStats.Services.Mapper.Interfaces;

using AutoMapper;

public interface ICustomMap
{
    void CreateMap(IProfileExpression profileExpression);
}