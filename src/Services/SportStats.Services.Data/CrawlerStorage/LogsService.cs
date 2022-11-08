namespace SportStats.Services.Data.CrawlerStorage;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Models.Entities.Crawlers;
using global::SportStats.Services.Data.CrawlerStorage.Interfaces;

using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<Guid>> GetLogIdentifiersAsync(int crawlerId)
    {
        var identifiers = await this.Context
            .Logs
            .Where(l => l.CrawlerId == crawlerId)
            .Select(l => l.Identifier)
            .ToListAsync();

        return identifiers;
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