namespace SportStats.Services.Data.SportStats.Interfaces;

public interface IAddable
{
    Task<TEntity> AddAsync<TEntity>(TEntity entity);
}