﻿namespace SportStats.Apps.ConsoleApp;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;
using SportStats.Data.Contexts;
using SportStats.Services;
using SportStats.Services.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = SetupConfiguration();
        var services = ConfigServices(configuration);

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
        //MapperConfig.RegisterMapper(typeof().GetTypeInfo().Assembly);

        // DATABASES
        var crawlerDbContextOptions = new DbContextOptionsBuilder<CrawlerDbContext>()
            .UseSqlServer(configuration.GetConnectionString(AppGlobalConstants.CRAWLER_NEW_CONNECTION_STRING))
            .Options;

        var sportStatsDbContextOptions = new DbContextOptionsBuilder<SportStatsDbContext>()
            .UseSqlServer(configuration.GetConnectionString(AppGlobalConstants.SPORT_STATS_CONNECTION_STRING))
            .UseLazyLoadingProxies()
            .Options;

        // ENGINE
        services.AddScoped<Engine>();

        // REPOSITORIES

        // SERVICES
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IZipService, ZipService>();

        // CRAWLERS

        // CONVERTERS

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}