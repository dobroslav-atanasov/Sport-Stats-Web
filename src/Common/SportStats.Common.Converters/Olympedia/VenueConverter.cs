namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Entities.SportStats;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Data.SportStats.Interfaces;
using SportStats.Services.Interfaces;

public class VenueConverter : BaseOlympediaConverter
{
    private readonly IVenuesService venuesService;

    public VenueConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegexService regexService, IDataCacheService dataCacheService, INormalizeService normalizeService, IVenuesService venuesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService, dataCacheService, normalizeService)
    {
        this.venuesService = venuesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var number = int.Parse(new Uri(group.Documents.Single().Url).Segments.Last());
            var fullName = document.DocumentNode.SelectSingleNode("//h1").InnerText.Decode();
            var name = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Name<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>")?.Decode();
            var city = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Place<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var englishName = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>English name<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>")?.Decode();
            var opened = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Opened<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
            var demolished = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Demolished<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
            var capacity = this.RegexService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Games Capacity<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");

            var venue = new OGVenue
            {
                CreatedOn = DateTime.UtcNow,
                Name = name,
                Number = number,
                City = this.NormalizeService.NormalizeHostCityName(city),
                EnglishName = englishName,
                FullName = fullName,
                Opened = opened != null ? int.Parse(opened) : null,
                Demolished = demolished != null ? int.Parse(demolished) : null,
                Capacity = capacity
            };

            var coordinatesMatch = this.RegexService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Coordinates<\/th>\s*<td>([\d\.\-]+),\s*([\d\.\-]+)");
            if (coordinatesMatch != null)
            {
                venue.LatitudeCoordinate = double.Parse(coordinatesMatch.Groups[1].Value);
                venue.LongitudeCoordinate = double.Parse(coordinatesMatch.Groups[2].Value);
            }

            var dbVenue = await this.venuesService.GetVenueAsync(venue.Number);
            if (dbVenue == null)
            {
                await this.venuesService.AddAsync(venue);
                this.Logger.LogInformation($"Added venue: {venue.Name}");
            }
            else
            {
                if (dbVenue.Update(venue))
                {
                    dbVenue.ModifiedOn = DateTime.UtcNow;
                    await this.venuesService.UpdateAsync(dbVenue);
                    this.Logger.LogInformation($"Updated venue: {venue.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}