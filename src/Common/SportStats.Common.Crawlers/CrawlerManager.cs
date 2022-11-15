namespace SportStats.Common.Crawlers;

using SportStats.Common.Crawlers.Countries;
using SportStats.Common.Crawlers.Olympedia;

public class CrawlerManager
{
	private readonly WorldCountryCrawler worldCountryCrawler;
	private readonly NOCCrawler olympediaNOCCrawler;
	private readonly GameCrawler olympediaGameCrawler;
	private readonly SportDisciplineCrawler olympediaSportDisciplineCrawler;
	private readonly ResultCrawler olympediaResultCrawler;
	private readonly AthleteCrawler olympediaAthleteCrawler;
	private readonly VenueCrawler olympediaVenueCrawler;

	public CrawlerManager(WorldCountryCrawler worldCountryCrawler,
		NOCCrawler olympediaNOCCrawler,
		GameCrawler olympediaGameCrawler,
		SportDisciplineCrawler olympediaSportDisciplineCrawler,
		ResultCrawler olympediaResultCrawler,
		AthleteCrawler olympediaAthleteCrawler,
		VenueCrawler olympediaVenueCrawler)
	{
		this.worldCountryCrawler = worldCountryCrawler;
		this.olympediaNOCCrawler = olympediaNOCCrawler;
		this.olympediaGameCrawler = olympediaGameCrawler;
		this.olympediaSportDisciplineCrawler = olympediaSportDisciplineCrawler;
		this.olympediaResultCrawler = olympediaResultCrawler;
		this.olympediaAthleteCrawler = olympediaAthleteCrawler;
		this.olympediaVenueCrawler = olympediaVenueCrawler;
	}

	public async Task RunWorldCountryCrawlers()
	{
		await this.worldCountryCrawler.StartAsync();
	}

	public async Task RunOlympediaCrawlers()
	{
		//await this.olympediaNOCCrawler.StartAsync();
		//await this.olympediaGameCrawler.StartAsync();
		//await this.olympediaSportDisciplineCrawler.StartAsync();
		//await this.olympediaResultCrawler.StartAsync();
		//await this.olympediaAthleteCrawler.StartAsync();
		await this.olympediaVenueCrawler.StartAsync();
	}
}