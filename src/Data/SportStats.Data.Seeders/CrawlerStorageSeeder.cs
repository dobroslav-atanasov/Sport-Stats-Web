namespace SportStats.Data.Seeders;

using System.Threading.Tasks;

using SportStats.Data.Models.Enumerations;
using SportStats.Data.Seeders.Interfaces;
using SportStats.Services.Data.CrawlerStorage.Interfaces;

public class CrawlerStorageSeeder : ISeeder
{
    private readonly IOperationsService operationsService;

    public CrawlerStorageSeeder(IOperationsService operationsService)
    {
        this.operationsService = operationsService;
    }

    public string SeederName => nameof(CrawlerStorageSeeder);

    public async Task SeedAsync()
    {
        if (!this.operationsService.IsOperationTableFull())
        {
            await this.operationsService.AddOperationAsync(OperationType.None.ToString());
            await this.operationsService.AddOperationAsync(OperationType.Add.ToString());
            await this.operationsService.AddOperationAsync(OperationType.Update.ToString());
            await this.operationsService.AddOperationAsync(OperationType.Delete.ToString());
            await this.operationsService.AddOperationAsync(OperationType.Error.ToString());
        }
    }
}