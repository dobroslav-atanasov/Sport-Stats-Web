namespace SportStats.Common.Converters.Olympedia;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class GameConverter : BaseOlympediaConverter
{
    private readonly IGamesService gamesService;

    public GameConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IGamesService gamesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService)
    {
        this.gamesService = gamesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var header = document.DocumentNode.SelectSingleNode("//h1").InnerText;

            var game = new OGGame { CreatedOn = DateTime.UtcNow };
            var gameMatch = this.RegexService.Match(header, @"(\d+)\s*(summer|winter)");
            if (gameMatch != null)
            {
                game.Year = int.Parse(gameMatch.Groups[1].Value);
                game.Type = gameMatch.Groups[2].Value.Trim().ToLower() == "summer" ? OlympicGameType.Summer : OlympicGameType.Winter;

                var numberMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<th>Number and Year<\/th>\s*<td>\s*([IVXLC]+)\s*\/(.*?)<\/td>");
                if (numberMatch != null)
                {
                    game.Number = numberMatch.Groups[1].Value.Trim();
                }

                var hostCityMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Host city<\/th>\s*<td>\s*([\w'\-\s.]+),\s*([\w'\-\s]+)");
                if (hostCityMatch != null)
                {
                    game.HostCity = this.NormalizeService.NormalizeHostCityName(hostCityMatch.Groups[1].Value.Trim());
                    var country = this.DataCacheService.OGCountriesCache.FirstOrDefault(c => c.Name == hostCityMatch.Groups[2].Value.Trim());
                    game.HostCountryId = country.Id;
                    game.OfficialName = this.SetOfficialName(game.HostCity, game.Year);
                }

                var openDateMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Opening ceremony<\/th>\s*<td>\s*([\d]+)\s*([A-Za-z]+)\s*(\d+)?\s*<\/td>\s*<\/tr>");
                if (openDateMatch != null)
                {
                    var day = int.Parse(openDateMatch.Groups[1].Value);
                    var month = openDateMatch.Groups[2].Value.GetMonthNumber();
                    game.OpenDate = DateTime.ParseExact($"{day}-{month}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
                }

                var closeDateMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Closing ceremony<\/th>\s*<td>\s*([\d]+)\s*([A-Za-z]+)\s*(\d+)?\s*<\/td>\s*<\/tr>");
                if (closeDateMatch != null)
                {
                    var day = int.Parse(closeDateMatch.Groups[1].Value);
                    var month = closeDateMatch.Groups[2].Value.GetMonthNumber();
                    game.CloseDate = DateTime.ParseExact($"{day}-{month}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
                }

                var competitionDateMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Competition dates<\/th>\s*<td>\s*(\d+)\s*([A-Za-z]+)?\s*–\s*(\d+)\s*([A-Za-z]+)\s*(\d+)?\s*<\/td>\s*<\/tr>");
                if (competitionDateMatch != null)
                {
                    var startDay = int.Parse(competitionDateMatch.Groups[1].Value);
                    var startMonth = competitionDateMatch.Groups[2].Value != string.Empty ? competitionDateMatch.Groups[2].Value.GetMonthNumber() : competitionDateMatch.Groups[4].Value.GetMonthNumber();
                    var endDay = int.Parse(competitionDateMatch.Groups[3].Value);
                    var endMonth = competitionDateMatch.Groups[4].Value.GetMonthNumber();

                    game.StartCompetitionDate = DateTime.ParseExact($"{startDay}-{startMonth}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
                    game.EndCompetitionDate = DateTime.ParseExact($"{endDay}-{endMonth}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
                }

                var openByMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Officially opened by<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (openByMatch != null)
                {
                    game.OpenBy = this.RegexService.CutHtml(openByMatch.Groups[1].Value);
                }

                var torchbearersMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Torchbearer\(s\)<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (torchbearersMatch != null)
                {
                    game.Torchbearers = this.RegexService.CutHtml(torchbearersMatch.Groups[1].Value);
                }

                var athleteOathByMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Taker of the Athlete's Oath<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (athleteOathByMatch != null)
                {
                    game.AthleteOathBy = this.RegexService.CutHtml(athleteOathByMatch.Groups[1].Value);
                }

                var judgeOathByMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Taker of the Official's Oath<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (judgeOathByMatch != null)
                {
                    game.JudgeOathBy = this.RegexService.CutHtml(judgeOathByMatch.Groups[1].Value);
                }

                var coachOathByMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Taker of the Coach's Oath<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (coachOathByMatch != null)
                {
                    game.CoachOathBy = this.RegexService.CutHtml(coachOathByMatch.Groups[1].Value);
                }

                var olympicFlagBearersMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Olympic Flag Bearers<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (olympicFlagBearersMatch != null)
                {
                    game.OlympicFlagBearers = this.RegexService.CutHtml(olympicFlagBearersMatch.Groups[1].Value);
                }

                var descriptionMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<h2>\s*Overview\s*(?:<small><\/small>)?\s*<\/h2>\s*<div class=(?:'|"")description(?:'|"")>\s*(.*?)<\/div>");
                if (descriptionMatch != null)
                {
                    game.Description = this.RegexService.CutHtml(descriptionMatch.Groups[1].Value);
                }

                var bidProcessMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<h2>Bid process<\/h2>\s*<div class=(?:'|"")description(?:'|"")>\s*(.*?)<\/div>");
                if (bidProcessMatch != null)
                {
                    game.BidProcess = this.RegexService.CutHtml(bidProcessMatch.Groups[1].Value);
                }

                var dbGame = await this.gamesService.GetGameAsync(game.Year, game.Type);
                if (dbGame == null)
                {
                    await this.gamesService.AddAsync(game);
                    this.Logger.LogInformation($"Added game: {game.OfficialName}");
                }
                else
                {
                    if (dbGame.Update(game))
                    {
                        dbGame.ModifiedOn = DateTime.UtcNow;
                        await this.gamesService.UpdateAsync(dbGame);
                        this.Logger.LogInformation($"Updated game: {game.OfficialName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private string SetOfficialName(string hostCity, int year)
    {
        if (hostCity == "Rio de Janeiro" && year == 2016)
        {
            hostCity = "Rio";
        }
        else if (hostCity == "Los Angeles" && year == 2028)
        {
            hostCity = "LA";
        }
        else if (hostCity == "Milano-Cortina d'Ampezzo" && year == 2026)
        {
            hostCity = "Milano Cortina";
        }

        return $"{hostCity} {year}";
    }
}