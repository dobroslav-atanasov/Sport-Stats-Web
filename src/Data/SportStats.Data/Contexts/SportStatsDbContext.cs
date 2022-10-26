namespace SportStats.Data.Contexts;

using Microsoft.EntityFrameworkCore;

public class SportStatsDbContext : DbContext
{
	public SportStatsDbContext(DbContextOptions<SportStatsDbContext> options)
		: base(options)
	{
	}


}