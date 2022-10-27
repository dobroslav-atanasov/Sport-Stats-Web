namespace SportStats.Repositories;

using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportStats.Data.Contexts;
using SportStats.Repositories.Interfaces;

public class SportStatsRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    public SportStatsRepository(SportStatsDbContext context)
    {
        this.Context = context;
    }

    protected SportStatsDbContext Context { get; }

    protected DbSet<TEntity> DbSet { get; set; }

    public async Task AddAsync(TEntity entity) => await this.DbSet.AddAsync(entity);

    public IQueryable<TEntity> All() => this.DbSet;

    public IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

    public void Delete(TEntity entity) => this.DbSet.Remove(entity);

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<TEntity> GetById(int id) => await this.DbSet.FindAsync(id);

    public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

    public void Update(TEntity entity) => this.Context.Entry(entity).State = EntityState.Modified;

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Context.Dispose();
        }
    }
}