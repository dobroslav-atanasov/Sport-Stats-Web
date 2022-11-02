namespace SportStats.Common.Crawlers.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class EventCrawler : BaseOlympediaCrawler
{
    public EventCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, httpService, crawlersService, groupsService)
    {
    }

    public async override Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(CrawlerConstants.OLYMPEDIA_GAMES_URL);
            var gameUrls = this.ExtractGameUrls(httpModel);

            foreach (var gameUrl in gameUrls)
            {
                try
                {
                    var gameHttpModel = await this.HttpService.GetAsync(gameUrl);
                    var disciplineUrls = this.ExtractOlympediaDisciplineUrls(gameHttpModel);

                    if (disciplineUrls != null && disciplineUrls.Count > 0)
                    {
                        var documents = new List<Document>
                            {
                                this.CreateDocument(gameHttpModel)
                            };
                        var count = 2;

                        foreach (var disciplineUrl in disciplineUrls)
                        {
                            var disciplineHttpModel = await this.HttpService.GetAsync(disciplineUrl);
                            var document = this.CreateDocument(disciplineHttpModel);
                            document.Order = count;
                            count++;
                            documents.Add(document);
                        }

                        await this.ProcessGroupAsync(gameHttpModel, documents);
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process data: {gameUrl};");
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