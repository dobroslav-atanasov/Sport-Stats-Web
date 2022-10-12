namespace SportStats.Apps.ConsoleApp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportStats.Common.Constants;

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

        // DATABASES

        // ENGINE
        services.AddScoped<Engine>();

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}