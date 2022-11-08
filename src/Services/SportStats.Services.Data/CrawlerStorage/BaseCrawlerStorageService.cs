namespace SportStats.Services.Data.CrawlerStorage;

using global::SportStats.Data.Contexts;

public abstract class BaseCrawlerStorageService
{
	public BaseCrawlerStorageService(CrawlerStorageDbContext context)
	{
		this.Context = context;
	}

	protected CrawlerStorageDbContext Context { get; }
}