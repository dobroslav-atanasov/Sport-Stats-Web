﻿namespace SportStats.Common.Crawlers.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class GameCrawler : BaseOlympediaCrawler
{
    public GameCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService, IConfiguration configuration)
        : base(logger, httpService, crawlersService, groupsService, configuration)
    {
    }

    public async override Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_GAMES_URL).Value);
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
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_GAMES_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}