namespace SportStats.Data.Common.Models;

using System.ComponentModel.DataAnnotations;

public abstract class BaseModel<TKey>
{
    [Key]
    public int Id { get; set; }
}