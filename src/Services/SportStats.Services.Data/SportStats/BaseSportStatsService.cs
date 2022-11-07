namespace SportStats.Services.Data.SportStats;

using global::SportStats.Data.Contexts;

public abstract class BaseSportStatsService
{
	public BaseSportStatsService(SportStatsDbContext context)
	{
		this.Context = context;
	}

	protected SportStatsDbContext Context { get; }
}