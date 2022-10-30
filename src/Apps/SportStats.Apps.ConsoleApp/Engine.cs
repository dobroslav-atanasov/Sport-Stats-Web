namespace SportStats.Apps.ConsoleApp;

using SportStats.Common.Crawlers;
using SportStats.Data.Seeders;
using SportStats.Data.Seeders.Interfaces;

internal class Engine
{
    private readonly CrawlerManager crawlerManager;
    private readonly IEnumerable<ISeeder> seeders;

    public Engine(CrawlerManager crawlerManager, IEnumerable<ISeeder> seeders)
    {
        this.crawlerManager = crawlerManager;
        this.seeders = seeders;
    }

    internal async Task RunAsync()
    {
        // SEEDERS
        var crawlerStorageSeeder = this.seeders.Single(s => s.SeederName == nameof(CrawlerStorageSeeder));
        await crawlerStorageSeeder.SeedAsync();

        // CRAWLERS
        await this.crawlerManager.RunWorldCountryCrawlers();

        // CONVERTERS
    }
}