namespace SportStats.Data.Common.Models;

public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}