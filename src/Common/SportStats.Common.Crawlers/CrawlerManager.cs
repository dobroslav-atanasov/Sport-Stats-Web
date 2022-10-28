namespace SportStats.Common.Crawlers;

using SportStats.Common.Crawlers.Countries;

public class CrawlerManager
{
	private readonly WorldCountryCrawler worldCountryCrawler;

	public CrawlerManager(WorldCountryCrawler worldCountryCrawler)
	{
		this.worldCountryCrawler = worldCountryCrawler;
	}

	public async Task RunWorldCountryCrawlers()
	{
		await this.worldCountryCrawler.StartAsync();
	}
}