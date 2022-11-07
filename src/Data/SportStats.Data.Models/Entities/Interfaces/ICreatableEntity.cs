namespace SportStats.Data.Models.Entities.Interfaces;

public interface ICreatableEntity
{
    DateTime CreatedOn { get; }

    DateTime? ModifiedOn { get; set; }
}