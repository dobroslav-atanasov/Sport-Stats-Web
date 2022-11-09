namespace SportStats.Data.Models.Entities.Interfaces;

public interface IUpdatable<T>
{
    bool Update(T other);
}