namespace SportStats.Apps.ConsoleApp;

using Microsoft.Extensions.Configuration;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = SetupConfiguration();
    }

    private static IConfiguration SetupConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile()
    }
}