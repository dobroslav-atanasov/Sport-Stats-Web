namespace SportStats.Services.Data.SportStats;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Entities.SportStats;
using global::SportStats.Services.Data.SportStats.Interfaces;

using Microsoft.EntityFrameworkCore;

public class SportsService : BaseSportStatsService, ISportsService
{
    public SportsService(SportStatsDbContext context) 
        : base(context)
    {
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    {
        await this.Context.AddAsync(entity);
        await this.Context.SaveChangesAsync();

        return entity;
    }

    public Task<OGSport> GetSportAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
    {
        this.Context.Entry(entity).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();

        return entity;
    }
}