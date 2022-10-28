namespace SportStats.Common.Crawlers;

using Microsoft.Extensions.Logging;

using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public abstract class BaseCrawler
{
	private readonly ICrawlersService crawlersService;
	private readonly Lazy<int> crawlerId;

	public BaseCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService)
	{
		this.Logger = logger;
		this.HttpService = httpService;
		this.crawlersService = crawlersService;
		this.crawlerId = new Lazy<int>(() => this.crawlersService.GetCrawlerIdAsync(this.GetType().FullName).GetAwaiter().GetResult());
	}

	protected ILogger<BaseCrawler> Logger { get; }

	protected IHttpService HttpService { get; }

	public abstract Task StartAsync();
}