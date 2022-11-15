namespace SportStats.Services.Data.SportStats.Interfaces;

public interface IUpdatable
{
    Task<TEntity> UpdateAsync<TEntity>(TEntity entity);
}