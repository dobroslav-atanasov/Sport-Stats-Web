namespace SportStats.Data.Models.Entities.Interfaces;

public interface ICreatableEntity
{
    DateTime CreatedOn { get; set; }

    DateTime? ModifiedOn { get; set; }
}