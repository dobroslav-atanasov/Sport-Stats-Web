﻿namespace SportStats.Common.Crawlers.Countries;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Http;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class WorldCountryCrawler : BaseCrawler
{
    public WorldCountryCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(CrawlerConstants.WORLD_COUNTRIES_URL);
            var countryUrls = this.ExtractCountryUrls(httpModel);

            foreach (var url in countryUrls)
            {
                try
                {
                    var countryHttpModel = await this.HttpService.GetAsync(url);
                    await this.ProcessGroupAsync(countryHttpModel);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {CrawlerConstants.WORLD_COUNTRIES_URL}");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }

    private IReadOnlyCollection<string> ExtractCountryUrls(HttpModel httpModel)
    {
        var urls = httpModel
            .HtmlDocument
            .DocumentNode
            .SelectNodes("//ul[@class='flag-grid']/li/a")
            .Select(x => x.Attributes["href"]?.Value.Trim())
            .Select(x => this.CreateUrl(x, CrawlerConstants.WORLD_COUNTRIES_MAIN_URL))
            .Distinct()
            .ToList();

        return urls;
    }
}