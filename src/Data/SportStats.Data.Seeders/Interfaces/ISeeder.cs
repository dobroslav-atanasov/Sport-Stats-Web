namespace SportStats.Data.Seeders.Interfaces;

public interface ISeeder
{
    string SeederName { get; }

    Task SeedAsync();
}