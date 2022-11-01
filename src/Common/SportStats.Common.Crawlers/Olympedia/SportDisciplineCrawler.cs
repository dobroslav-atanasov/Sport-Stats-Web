namespace SportStats.Common.Crawlers.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class SportDisciplineCrawler : BaseOlympediaCrawler
{
    public SportDisciplineCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService) 
        : base(logger, httpService, crawlersService, groupsService)
    {
    }

    public async override Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(CrawlerConstants.OLYMPEDIA_SPORTS_URL);
            await this.ProcessGroupAsync(httpModel);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {CrawlerConstants.OLYMPEDIA_SPORTS_URL};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}