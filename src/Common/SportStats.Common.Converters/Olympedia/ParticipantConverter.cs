namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Models.Cache.OlympicGames;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class ParticipantConverter : BaseOlympediaConverter
{
    private readonly IAthletesService athletesService;
    private readonly IParticipantsService participantsService;
    private readonly ITeamsService teamsService;
    private readonly ISquadsService squadsService;

    public ParticipantConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IOlympediaService olympediaService, IAthletesService athletesService,
        IParticipantsService participantsService, ITeamsService teamsService, ISquadsService squadsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService, olympediaService)
    {
        this.athletesService = athletesService;
        this.participantsService = participantsService;
        this.teamsService = teamsService;
        this.squadsService = squadsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single(x => x.Order == 1));
            var originalEventName = document.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']/li[@class='active']").InnerText;
            var game = this.FindGame(document);
            var discipline = this.FindDiscipline(document);
            var eventModel = this.CreateEventModel(originalEventName, game, discipline);
            if (eventModel != null && !this.CheckForbiddenEvent(eventModel.OriginalName, discipline.Name, game.Year))
            {
                var @event = this.DataCacheService
                    .EventCacheModels
                    .FirstOrDefault(x => x.OriginalName == eventModel.OriginalName && x.GameId == eventModel.GameId && x.DisciplineId == eventModel.DisciplineId);

                if (@event != null)
                {
                    var trRows = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']").Elements("tr");
                    var indexes = this.GetHeaderIndexes(document);
                    if (@event.IsTeamEvent)
                    {
                        OGTeam team = null;
                        foreach (var trRow in trRows)
                        {
                            var tdNodes = trRow.Elements("td").ToList();
                            var countryCode = this.OlympediaService.FindCountryCode(trRow.OuterHtml);
                            if (countryCode != null && !trRow.InnerHtml.ToLower().Contains("coach"))
                            {
                                var country = this.DataCacheService.CountryCacheModels.FirstOrDefault(x => x.Code == countryCode);
                                team = new OGTeam
                                {
                                    CreatedOn = DateTime.UtcNow,
                                    CountryId = country.Id,
                                    EventId = @event.Id,
                                    CountryName = country.Name,
                                    Name = tdNodes[indexes[ConverterConstants.INDEX_NAME]].InnerText.Trim(),
                                    Medal = this.OlympediaService.FindMedal(trRow.OuterHtml)
                                };

                                var dbTeam = await this.teamsService.GetTeamAsync(team.Name, team.EventId, team.CountryId);
                                if (dbTeam == null)
                                {
                                    team = await this.teamsService.AddAsync(team);
                                    this.Logger.LogInformation($"Added team: name - {team.Name}, event - {team.EventId}");
                                }
                                else
                                {
                                    if (dbTeam.Update(team))
                                    {
                                        dbTeam.ModifiedOn = DateTime.UtcNow;
                                        dbTeam = await this.teamsService.UpdateAsync(dbTeam);
                                        this.Logger.LogInformation($"Updated team: name - {team.Name}, event - {team.EventId}");
                                    }
                                }

                                team = dbTeam ?? team;
                            }

                            if (trRow.InnerHtml.ToLower().Contains("coach"))
                            {
                                var athleteNumber = this.OlympediaService.FindAthleteNumber(trRow.OuterHtml);
                                var coach = await this.athletesService.GetAthleteByNumberAsync(athleteNumber);
                                if (coach != null)
                                {
                                    var dbTeam = await this.teamsService.GetTeamAsync(team.Name, team.EventId, team.CountryId);
                                    team.CoachId = coach.Id;
                                    if (dbTeam.Update(team))
                                    {
                                        dbTeam.ModifiedOn = DateTime.UtcNow;
                                        await this.teamsService.UpdateAsync(dbTeam);
                                        this.Logger.LogInformation($"Updated team: name - {team.Name}, event - {team.EventId}");
                                    }
                                }
                            }
                            else
                            {
                                var athleteNumbers = this.OlympediaService.FindAthleteNumbers(trRow.OuterHtml);
                                var currentCountry = this.DataCacheService.CountryCacheModels.FirstOrDefault(x => x.Name == team.CountryName);
                                foreach (var athleteNumber in athleteNumbers)
                                {
                                    var participant = await this.CreateParticipantAsync(athleteNumber, currentCountry.Code, team.Medal, @event, game);

                                    if (participant != null && !this.squadsService.SquadExists(participant.Id, team.Id))
                                    {
                                        await this.squadsService.AddAsync(new OGSquad { ParticipantId = participant.Id, TeamId = team.Id });
                                        this.Logger.LogInformation($"Added squad: participant - {participant.Id}, team - {team.Id}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var trRow in trRows)
                        {
                            var athleteNumber = this.OlympediaService.FindAthleteNumber(trRow.OuterHtml);
                            var countryCode = this.OlympediaService.FindCountryCode(trRow.OuterHtml);
                            var medalType = this.OlympediaService.FindMedal(trRow.OuterHtml);
                            if (athleteNumber != 0 && countryCode != null)
                            {
                                await this.CreateParticipantAsync(athleteNumber, countryCode, medalType, @event, game);
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

    private async Task<OGParticipant> CreateParticipantAsync(int athleteNumber, string countryCode, MedalType medalType, EventCacheModel @event, GameCacheModel game)
    {
        var athlete = await this.athletesService.GetAthleteByNumberAsync(athleteNumber);
        if (athlete != null)
        {
            var country = this.DataCacheService.CountryCacheModels.FirstOrDefault(c => c.Code == countryCode);
            if (country != null)
            {
                var participant = new OGParticipant
                {
                    CreatedOn = DateTime.UtcNow,
                    AthleteId = athlete.Id,
                    EventId = @event.Id,
                    OlympediaNumber = athleteNumber,
                    Country = country.Name,
                    Medal = medalType
                };

                if (athlete.BirthDate.HasValue)
                {
                    this.CalculateAge(game.OpenDate ?? game.StartCompetitionDate.Value, athlete.BirthDate.Value, participant);
                }

                var dbParticipant = await this.participantsService.GetParticipantAsync(participant.AthleteId, participant.EventId);
                if (dbParticipant == null)
                {
                    await this.participantsService.AddAsync(participant);
                    this.Logger.LogInformation($"Added participant: athlete - {participant.AthleteId}, event - {participant.EventId}");
                }
                else
                {
                    if (dbParticipant.Update(participant))
                    {
                        dbParticipant.ModifiedOn = DateTime.UtcNow;
                        await this.participantsService.UpdateAsync(dbParticipant);
                        this.Logger.LogInformation($"Updated participant: athlete - {participant.AthleteId}, event - {participant.EventId}");
                    }
                }

                return dbParticipant ?? participant;
            }
        }

        return null;
    }

    private void CalculateAge(DateTime startDate, DateTime endDate, OGParticipant participant)
    {
        var totalDays = (startDate - endDate).TotalDays;
        var year = (int)Math.Floor(totalDays / 365.25);
        var newYear = endDate.AddYears(year);
        var days = (startDate - newYear).TotalDays;

        participant.AgeYears = year;
        participant.AgeDays = (int)days;
    }
}