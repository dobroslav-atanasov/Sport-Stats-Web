namespace SportStats.Common.Crawlers.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class GameCrawler : BaseOlympediaCrawler
{
    public GameCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, httpService, crawlersService, groupsService)
    {
    }

    public async override Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(CrawlerConstants.OLYMPEDIA_GAMES_URL);
            var urls = this.ExtractGameUrls(httpModel);

            foreach (var url in urls)
            {
                try
                {
                    var gameHttpModel = await this.HttpService.GetAsync(url);
                    await this.ProcessGroupAsync(gameHttpModel);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process data: {url};");
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {CrawlerConstants.OLYMPEDIA_GAMES_URL};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}