namespace SportStats.Data.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class SportStatsDbContext : IdentityDbContext
{
    public SportStatsDbContext(DbContextOptions<SportStatsDbContext> options)
        : base(options)
    {
    }
}