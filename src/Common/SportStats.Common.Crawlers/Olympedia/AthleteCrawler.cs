namespace SportStats.Common.Crawlers.Olympedia;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Http;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class AthleteCrawler : BaseOlympediaCrawler
{
    public AthleteCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, httpService, crawlersService, groupsService)
    {
    }

    public async override Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");
        var groups = await this.GroupsService.GetGroupNamesAsync(this.CrawlerId.Value);

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
                                    var athletetUrls = this.ExtractAthleteUrls(mainResultHttpModel);

                                    foreach (var athleteUrl in athletetUrls)
                                    {
                                        var number = Regex.Match(athleteUrl, @"athletes/(\d+)");
                                        if (!groups.Contains($"athletes_{number.Groups[1].Value}.zip"))
                                        {
                                            try
                                            {
                                                var athleteHttpModel = await this.HttpService.GetAsync(athleteUrl);
                                                groups.Add(athleteUrl);
                                                await this.ProcessGroupAsync(athleteHttpModel);
                                            }
                                            catch (Exception ex)
                                            {
                                                this.Logger.LogError(ex, $"Failed to download data: {athleteUrl};");
                                            }
                                        }
                                    }

                                    var resultUrls = this.ExtractResultUrls(mainResultHttpModel);
                                    foreach (var resultUrl in resultUrls)
                                    {
                                        var resultHttpModel = await this.HttpService.GetAsync(resultUrl);
                                        athletetUrls = this.ExtractAthleteUrls(resultHttpModel);

                                        if (athletetUrls != null)
                                        {
                                            foreach (var athleteUrl in athletetUrls)
                                            {
                                                var number = Regex.Match(athleteUrl, @"athletes/(\d+)");
                                                if (!groups.Contains($"athletes_{number.Groups[1].Value}.zip"))
                                                {
                                                    try
                                                    {
                                                        var athleteHttpModel = await this.HttpService.GetAsync(athleteUrl);
                                                        groups.Add(athleteUrl);
                                                        await this.ProcessGroupAsync(athleteHttpModel);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        this.Logger.LogError(ex, $"Failed to process data: {athleteUrl};");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.Logger.LogError(ex, $"Failed to process data: {disciplineUrl};");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogError(ex, $"Failed to process data: {disciplineUrl};");
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
            this.Logger.LogError(ex, $"Failed to process url: {CrawlerConstants.OLYMPEDIA_GAMES_URL};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }

    private IReadOnlyCollection<string> ExtractAthleteUrls(HttpModel httpModel)
    {
        var urls = httpModel
            .HtmlDocument
            .DocumentNode?
            .SelectNodes("//div[@class='container']//a")?
            .Select(x => x.Attributes["href"]?.Value.Trim())
            .Where(x => x.StartsWith("/athletes/"))
            .Select(x => this.CreateUrl(x, CrawlerConstants.OLYMPEDIA_MAIN_URL))
            .Distinct()
            .ToList();

        return urls;
    }
}