namespace SportStats.Services.Data.CrawlerStorage;

using SportStats.Data.Contexts;

public abstract class BaseCrawlerStorageService
{
	public BaseCrawlerStorageService(CrawlerStorageDbContext context)
	{
		this.Context = context;
	}

	protected CrawlerStorageDbContext Context { get; }
}