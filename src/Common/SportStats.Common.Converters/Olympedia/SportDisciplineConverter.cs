namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class SportDisciplineConverter : BaseOlympediaConverter
{
    private readonly ISportsService sportsService;
    private readonly IDisciplinesService disciplinesService;

    public SportDisciplineConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService,
        IZipService zipService, IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, ISportsService sportsService,
        IDisciplinesService disciplinesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService)
    {
        this.sportsService = sportsService;
        this.disciplinesService = disciplinesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var lines = document.DocumentNode.SelectNodes("//table[@class='table table-striped sortable']//tr");

            foreach (var line in lines)
            {
                if (line.OuterHtml.Contains("glyphicon-ok"))
                {
                    var elements = line.Elements("td").ToList();
                    var sportName = elements[2].InnerText.Trim();
                    if (sportName != "Air Sports" && sportName != "Mountaineering and Climbing" && sportName != "Art Competitions")
                    {
                        var type = elements[3].InnerText.Trim().ToEnum<OlympicGameType>();
                        var sportAbbreviation = this.RegexService.MatchFirstGroup(elements[2].OuterHtml, @"/sport_groups/(.*?)""");
                        var sport = new OGSport
                        {
                            CreatedOn = DateTime.UtcNow,
                            Name = sportName,
                            Type = type,
                            Abbreviation = sportAbbreviation
                        };

                        var dbSport = await this.sportsService.GetSportAsync(sport.Name);
                        if (dbSport == null)
                        {
                            await this.sportsService.AddAsync(sport);
                            this.Logger.LogInformation($"Added sport: {sport.Name}");
                        }
                        else
                        {
                            if (dbSport.Update(sport))
                            {
                                dbSport.ModifiedOn = DateTime.UtcNow;
                                await this.sportsService.UpdateAsync(dbSport);
                                this.Logger.LogInformation($"Updated sport: {dbSport.Name}");
                            }
                        }

                        var disciplineName = elements[1].InnerText.Trim();
                        var disciplineAbbreviation = elements[0].InnerText.Trim();
                        var disciplines = new List<OGDiscipline>();
                        if (sport.Name == "Wrestling")
                        {
                            disciplines.Add(new OGDiscipline
                            {
                                CreatedOn = DateTime.UtcNow,
                                Name = "Wrestling Freestyle",
                                Abbreviation = "WRF",
                                SportId = dbSport != null ? dbSport.Id : sport.Id
                            });

                            disciplines.Add(new OGDiscipline
                            {
                                CreatedOn = DateTime.UtcNow,
                                Name = "Wrestling Greco-Roman",
                                Abbreviation = "WRG",
                                SportId = dbSport != null ? dbSport.Id : sport.Id
                            });
                        }
                        else
                        {
                            disciplines.Add(new OGDiscipline
                            {
                                CreatedOn = DateTime.UtcNow,
                                Name = disciplineName,
                                Abbreviation = disciplineAbbreviation,
                                SportId = dbSport != null ? dbSport.Id : sport.Id
                            });
                        }

                        foreach (var discipline in disciplines)
                        {
                            var dbDiscipline = await this.disciplinesService.GetDisciplineAsync(discipline.Name);
                            if (dbDiscipline == null)
                            {
                                await this.disciplinesService.AddAsync(discipline);
                                this.Logger.LogInformation($"Added discipline: {discipline.Name}");
                            }
                            else
                            {
                                if (dbDiscipline.Update(discipline))
                                {
                                    dbDiscipline.ModifiedOn = DateTime.UtcNow;
                                    await this.disciplinesService.UpdateAsync(dbDiscipline);
                                    this.Logger.LogInformation($"Updated discipline: {dbDiscipline.Name}");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}