namespace SportStats.Apps.ConsoleApp;

using SportStats.Common.Converters;
using SportStats.Common.Crawlers;
using SportStats.Data.Seeders;
using SportStats.Data.Seeders.Interfaces;

internal class Engine
{
    private readonly CrawlerManager crawlerManager;
    private readonly ConverterManager converterManager;
    private readonly IEnumerable<ISeeder> seeders;

    public Engine(CrawlerManager crawlerManager, ConverterManager converterManager, IEnumerable<ISeeder> seeders)
    {
        this.crawlerManager = crawlerManager;
        this.converterManager = converterManager;
        this.seeders = seeders;
    }

    internal async Task RunAsync()
    {
        // SEEDERS
        var crawlerStorageSeeder = this.seeders.Single(s => s.SeederName == nameof(CrawlerStorageSeeder));
        await crawlerStorageSeeder.SeedAsync();

        // CRAWLERS
        //await this.crawlerManager.RunWorldCountryCrawlers();
        await this.crawlerManager.RunOlympediaCrawlers();

        // CONVERTERS
        //await this.converterManager.RunWorldCountriesConverters();
        //await this.converterManager.RunOlympediaConverters();
    }
}