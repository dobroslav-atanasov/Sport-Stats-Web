namespace SportStats.Common.Crawlers;

using Microsoft.Extensions.Logging;

using SportStats.Services.Interfaces;

public abstract class BaseCrawler
{
	public BaseCrawler(ILogger<BaseCrawler> logger, IHttpService httpService)
	{
		this.Logger = logger;
		this.HttpService = httpService;
	}

	protected ILogger<BaseCrawler> Logger { get; }

	protected IHttpService HttpService { get; }

	public abstract Task StartAsync();
}