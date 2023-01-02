namespace SportStats.Services.Data.SportStats;

using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Cache.OlympicGames;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;
using global::SportStats.Services.Mapper.Extensions;

using Microsoft.EntityFrameworkCore;

public class DisciplinesService : BaseSportStatsService, IDisciplinesService
{
    public DisciplinesService(SportStatsDbContext context)
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public async Task<OGDiscipline> GetDisciplineAsync(string name)
    {
        var discipline = await this.Context
            .OGDisciplines
            .FirstOrDefaultAsync(d => d.Name == name);

        return discipline;
    }

    public ICollection<DisciplineCacheModel> GetDisciplineCacheModels()
    {
        var disciplines = this.Context
            .OGDisciplines
            .AsNoTracking()
            .To<DisciplineCacheModel>()
            .ToList();

        return disciplines;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        this.Context.Entry(entity).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return entity;
    }
}