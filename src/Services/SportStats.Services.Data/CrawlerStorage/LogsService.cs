namespace SportStats.Services.Data.CrawlerStorage;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportStats.Data.Contexts;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;

public class LogsService : BaseCrawlerStorageService, ILogsService
{
    public LogsService(CrawlerStorageDbContext context)
        : base(context)
    {
    }

    public async Task AddLogAsync(Log log)
    {
        await this.Context.AddAsync(log);
        await this.Context.SaveChangesAsync();
    }

    public async Task UpdateLogAsync(Guid identifier, int operation)
    {
        var log = await this.Context
            .Logs
            .FirstOrDefaultAsync(l => l.Identifier == identifier);

        if (log != null)
        {
            log.Operation = operation;
            this.Context.Entry(log).State = EntityState.Modified;
            await this.Context.SaveChangesAsync();
        }
    }
}