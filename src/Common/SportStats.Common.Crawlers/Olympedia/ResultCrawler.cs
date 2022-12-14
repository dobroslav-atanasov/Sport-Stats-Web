namespace SportStats.Common.Crawlers.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class ResultCrawler : BaseOlympediaCrawler
{
    public ResultCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService, IConfiguration configuration)
        : base(logger, httpService, crawlersService, groupsService, configuration)
    {
    }

    public async override Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_GAMES_URL).Value);
            var gameUrls = this.ExtractGameUrls(httpModel);

            foreach (var gameUrl in gameUrls)
            {
                try
                {
                    var gameHttpModel = await this.HttpService.GetAsync(gameUrl);
                    var disciplineUrls = this.ExtractOlympediaDisciplineUrls(gameHttpModel);

                    if (disciplineUrls != null && disciplineUrls.Count > 0)
                    {
                        foreach (var disciplineUrl in disciplineUrls)
                        {
                            try
                            {
                                var disciplineModel = await this.HttpService.GetAsync(disciplineUrl);
                                var medalDisciplineUrls = this.GetMedalDisciplineUrls(disciplineModel);

                                foreach (var medalDisciplineUrl in medalDisciplineUrls)
                                {
                                    try
                                    {
                                        var mainResultHttpModel = await this.HttpService.GetAsync(medalDisciplineUrl);
                                        var resultUrls = this.ExtractResultUrls(mainResultHttpModel);

                                        var documents = new List<Document>
                                    {
                                        this.CreateDocument(mainResultHttpModel)
                                    };
                                        var order = 2;

                                        foreach (var resultUrl in resultUrls)
                                        {
                                            try
                                            {
                                                var resultHttpModel = await this.HttpService.GetAsync(resultUrl);
                                                var document = this.CreateDocument(resultHttpModel);
                                                document.Order = order;
                                                order++;
                                                documents.Add(document);
                                            }
                                            catch (Exception ex)
                                            {
                                                this.Logger.LogError(ex, $"Failed to process data: {resultUrl};");
                                            }
                                        }

                                        await this.ProcessGroupAsync(mainResultHttpModel, documents);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.Logger.LogError(ex, $"Failed to process data: {medalDisciplineUrl};");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Logger.LogError(ex, $"Failed to process data: {disciplineUrl};");
                            }
                        }
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
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_GAMES_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}