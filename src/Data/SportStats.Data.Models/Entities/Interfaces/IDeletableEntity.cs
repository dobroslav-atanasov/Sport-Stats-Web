namespace SportStats.Data.Models.Entities.Interfaces;

public interface IDeletableEntity
{
    bool IsDeleted { get; set; }

    DateTime? DeletedOn { get; set; }
}