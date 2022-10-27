namespace SportStats.Common.Converters;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;

public abstract class BaseConverter
{
    public BaseConverter(ILogger<BaseConverter> logger)
    {
        this.Logger = logger;
    }

    protected ILogger<BaseConverter> Logger { get; }

    protected abstract Task ProcessGroupAsync(Group group);

    public async Task ConvertAsync(string converterName)
    {
        //this.Logger.LogInformation($"Converter: {converterName} start.");

        //try
        //{
        //    // CONVERT ONLY 1 GUID
        //    //var groupDto = await this.ConverterService.GetGroupAsync(Guid.Parse("FF0E2B2A-45F9-4E39-9744-C65D9CA5AE0B"));
        //    //await this.ProcessGroupAsync(groupDto);

        //    var crawler = await this.ConverterService.GetCrawlerAsync(converterName);
        //    var identifiers = await this.ConverterService.GetIdentifiersAsync(crawler.Id);

        //    //identifiers = new List<Guid>
        //    //{
        //    //    Guid.Parse("FD8B285D-C165-4407-9019-871464D17A02")
        //    //};

        //    await identifiers.ParallelForEachAsync(async identifier =>
        //    {
        //        try
        //        {
        //            var group = await this.ConverterService.GetGroupAsync(identifier);
        //            await this.ProcessGroupAsync(group);
        //        }
        //        catch (Exception ex)
        //        {
        //            this.Logger.LogError(ex, $"Group was not process: {identifier};");
        //        }
        //    }, maxDegreeOfParallelism: 1);
        //}
        //catch (Exception ex)
        //{
        //    this.Logger.LogError(ex, $"Failed to process documents from converter: {converterName};");
        //}

        //this.Logger.LogInformation($"Converter: {converterName} end.");
    }
}