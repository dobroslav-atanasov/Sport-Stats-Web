namespace SportStats.Common.Crawlers;

using SportStats.Common.Crawlers.Countries;
using SportStats.Common.Crawlers.Olympedia;

public class CrawlerManager
{
	private readonly WorldCountryCrawler worldCountryCrawler;
	private readonly NOCCrawler olympediaNOCCrawler;

	public CrawlerManager(WorldCountryCrawler worldCountryCrawler,
		NOCCrawler olympediaNOCCrawler)
	{
		this.worldCountryCrawler = worldCountryCrawler;
		this.olympediaNOCCrawler = olympediaNOCCrawler;
	}

	public async Task RunWorldCountryCrawlers()
	{
		//await this.worldCountryCrawler.StartAsync();
		await this.olympediaNOCCrawler.StartAsync();
	}
}