namespace SportStats.Services.Data.CrawlerStorage;

using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Entities.Crawlers;
using global::SportStats.Services.Data.CrawlerStorage.Interfaces;

public class OperationsService : BaseCrawlerStorageService, IOperationsService
{
    public OperationsService(CrawlerStorageDbContext context)
        : base(context)
    {
    }

    public async Task AddOperationAsync(string operationName)
    {
        var operation = new Operation { Name = operationName };
        await this.Context.AddAsync(operation);
        await this.Context.SaveChangesAsync();
    }

    public bool IsOperationTableFull()
    {
        return this.Context.Operations.Any();
    }
}