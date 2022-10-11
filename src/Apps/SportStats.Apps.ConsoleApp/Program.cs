namespace SportStats.Apps.ConsoleApp;

using Microsoft.Extensions.Configuration;

using SportStats.Common.Constants;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = SetupConfiguration();
    }

    private static IConfiguration SetupConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(AppGlobalConstants.APP_SETTINGS_FILE, false, true)
            .Build();

        return configuration;
    }
}