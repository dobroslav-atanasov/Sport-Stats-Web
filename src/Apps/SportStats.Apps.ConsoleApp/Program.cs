namespace SportStats.Apps.ConsoleApp;

using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Common.Crawlers;
using SportStats.Common.Crawlers.Countries;
using SportStats.Data.Contexts;
using SportStats.Data.Seeders;
using SportStats.Data.Seeders.Interfaces;
using SportStats.Services;
using SportStats.Services.Data.CrawlerStorage;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;
using SportStats.Services.Mapper;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = SetupConfiguration();
        var services = ConfigServices(configuration);

        // MIGRATIONS
        var crawlerStorageDbContext = services.GetRequiredService<CrawlerStorageDbContext>();
        crawlerStorageDbContext.Database.Migrate();

        var sportStatsDbContext = services.GetRequiredService<SportStatsDbContext>();
        sportStatsDbContext.Database.Migrate();

        var engine = services.GetService<Engine>();
        if (engine != null)
        {
            await engine.RunAsync();
        }
    }

    private static IConfiguration SetupConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(AppGlobalConstants.APP_SETTINGS_FILE, false, true)
            .Build();

        return configuration;
    }

    private static ServiceProvider ConfigServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        // LOGGING
        services.AddLogging(config =>
        {
            config.AddConfiguration(configuration.GetSection(AppGlobalConstants.LOGGING));
            config.AddConsole();
            config.AddLog4Net(configuration.GetSection(AppGlobalConstants.LOG4NET_CORE).Get<Log4NetProviderOptions>());
        });

        // AUTOMAPPER
        MapperConfig.RegisterMapper(Assembly.Load(AppGlobalConstants.AUTOMAPPER_MODELS_ASSEMBLY));

        // DATABASES
        services.AddDbContext<CrawlerStorageDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(AppGlobalConstants.CRAWLER_STORAGE_CONNECTION_STRING));
            options.UseLazyLoadingProxies();
        });

        services.AddDbContext<SportStatsDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(AppGlobalConstants.SPORT_STATS_CONNECTION_STRING));
            options.UseLazyLoadingProxies();
        });

        // ENGINE
        services.AddScoped<Engine>();

        // SEEDERS
        services.AddTransient<ISeeder, CrawlerStorageSeeder>();

        // SERVICES
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IZipService, ZipService>();
        services.AddScoped<IMD5Hash, MD5Hash>();

        // SERVICES DATA
        services.AddScoped<ICrawlersService, CrawlersService>();
        services.AddScoped<IOperationsService, OperationsService>();
        services.AddScoped<IGroupsService, GroupsService>();
        services.AddScoped<ILogsService, LogsService>();

        // CRAWLERS
        services.AddTransient<CrawlerManager>();
        services.AddTransient<WorldCountryCrawler>();

        // CONVERTERS

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}