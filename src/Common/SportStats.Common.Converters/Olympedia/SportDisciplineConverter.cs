namespace SportStats.Common.Converters.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class SportDisciplineConverter : BaseConverter
{
    public SportDisciplineConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, 
        IZipService zipService, IRegexService regexService) 
        : base(logger, crawlersService, logsService, groupsService, zipService, regexService)
    {
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
                    var elements = line.Elements("td");
                    ;
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}