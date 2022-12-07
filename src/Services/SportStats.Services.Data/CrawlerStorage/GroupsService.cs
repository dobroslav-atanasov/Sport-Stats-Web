namespace SportStats.Services.Data.CrawlerStorage;

using System.Collections.Generic;
using System.Threading.Tasks;

using global::SportStats.Data.Contexts;
using global::SportStats.Data.Factory.Interfaces;
using global::SportStats.Data.Models.Entities.Crawlers;
using global::SportStats.Data.Models.Enumerations;
using global::SportStats.Services.Data.CrawlerStorage.Interfaces;
using global::SportStats.Services.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class GroupsService : BaseCrawlerStorageService, IGroupsService
{
    private readonly ILogger<GroupsService> logger;
    private readonly IZipService zipService;
    private readonly IMD5Hash hasher;
    private readonly ILogsService logsService;
    private readonly IDbContextFactory dbContextFactory;

    public GroupsService(CrawlerStorageDbContext context, ILogger<GroupsService> logger, IZipService zipService, IMD5Hash hasher, ILogsService logsService,
        IDbContextFactory dbContextFactory)
        : base(context)
    {
        this.logger = logger;
        this.zipService = zipService;
        this.hasher = hasher;
        this.logsService = logsService;
        this.dbContextFactory = dbContextFactory;
    }

    public async Task AddGroupAsync(Group group)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();
        await context.AddAsync(group);
        await context.SaveChangesAsync();
    }

    public async Task AddOrUpdateGroupAsync(Group group)
    {
        foreach (var document in group.Documents)
        {
            document.Identifier = Guid.NewGuid();
            document.MD5 = this.hasher.Hash(document.Content);
            document.Operation = (int)OperationType.Add;
        }

        var folder = group.Name.ToLower();
        var zipFolderName = $"{folder}.zip";
        group.Name = zipFolderName;
        group.Identifier = Guid.NewGuid();
        group.Date = DateTime.UtcNow;
        group.Operation = (int)OperationType.Add;
        group.Content = this.zipService.ZipGroup(group);

        var dbGroup = await this.GetGroupAsync(group.CrawlerId, group.Name);

        if (dbGroup == null)
        {
            try
            {
                await this.AddGroupAsync(group);
                var log = new Log
                {
                    Identifier = group.Identifier,
                    LogDate = DateTime.UtcNow,
                    Operation = (int)OperationType.Add,
                    CrawlerId = group.CrawlerId,
                };
                await this.logsService.AddLogAsync(log);
                this.logger.LogInformation($"Add group: {group.Identifier} - {group.Name}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to add group: {group.Identifier}, {group.Name};");
            }
        }
        else
        {
            var isUpdated = this.CheckForUpdate(group, dbGroup);
            if (isUpdated)
            {
                try
                {
                    await this.UpdateGroupAsync(group, dbGroup);
                    await this.logsService.UpdateLogAsync(dbGroup.Identifier, (int)OperationType.Update);
                    this.logger.LogInformation($"Update group: {dbGroup.Identifier} - {dbGroup.Name}");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Failed to update group: {dbGroup.Identifier}, {dbGroup.Name};");
                }
            }
            else
            {
                try
                {
                    await this.logsService.UpdateLogAsync(dbGroup.Identifier, (int)OperationType.None);
                    this.logger.LogInformation($"Тhe group has not changed: {dbGroup.Identifier} - {dbGroup.Name}");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Failed to not changed group: {dbGroup.Identifier}, {dbGroup.Name};");
                }
            }
        }
    }

    public async Task<Group> GetGroupAsync(int crawlerId, string name)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var group = await context
            .Groups
            .FirstOrDefaultAsync(g => g.CrawlerId == crawlerId && g.Name == name);

        return group;
    }

    public async Task<Group> GetGroupAsync(Guid identifier)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var group = await context
            .Groups
            .Include(x => x.Documents)
            .FirstOrDefaultAsync(g => g.Identifier == identifier);

        return group;
    }

    public async Task<IList<string>> GetGroupNamesAsync(int crawlerId)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var groups = await context
            .Groups
            .Where(g => g.CrawlerId == crawlerId)
            .Select(g => g.Name)
            .ToListAsync();

        return groups;
    }

    public async Task UpdateGroupAsync(Group newGroup, Group oldGroup)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var group = await context
            .Groups
            .SingleAsync(g => g.Identifier == oldGroup.Identifier);

        await context
            .Entry(group)
            .Collection(g => g.Documents)
            .LoadAsync();

        group.Operation = (int)OperationType.Update;
        group.Content = newGroup.Content;
        group.Date = DateTime.UtcNow;

        foreach (var document in newGroup.Documents)
        {
            if (document.Operation == (int)OperationType.Add)
            {
                document.Operation = (int)OperationType.Add;
                group.Documents.Add(document);
            }

            if (document.Operation == (int)OperationType.Update)
            {
                var doc = group.Documents.Single(d => d.Name == document.Name);
                doc.Operation = (int)OperationType.Update;
                doc.Format = document.Format;
                doc.Url = document.Url;
                doc.MD5 = document.MD5;
            }

            if (document.Operation == (int)OperationType.None)
            {
                var doc = group.Documents.Single(d => d.Name == document.Name);
                doc.Operation = (int)OperationType.None;
            }
        }

        foreach (var document in oldGroup.Documents)
        {
            if (document.Operation == (int)OperationType.Delete)
            {
                var doc = group.Documents.FirstOrDefault(d => d.Name == document.Name);
                if (doc != null)
                {
                    doc.Operation = document.Operation;
                }
            }
        }

        await context.SaveChangesAsync();
    }

    private bool CheckForUpdate(Group group, Group dbGroup)
    {
        var isUpdated = false;
        foreach (var document in group.Documents)
        {
            var dbDocumentViewModel = dbGroup
                .Documents
                .Where(x => x.Name == document.Name)
                .FirstOrDefault();

            if (dbDocumentViewModel == null)
            {
                document.Operation = (int)OperationType.Add;
                isUpdated = true;
            }
            else
            {
                if (document.MD5 != dbDocumentViewModel.MD5 || document.Format != dbDocumentViewModel.Format)
                {
                    document.Operation = (int)OperationType.Update;
                    dbDocumentViewModel.Operation = (int)OperationType.Update;
                    isUpdated = true;
                }
            }
        }

        foreach (var document in dbGroup.Documents)
        {
            if (!group.Documents.Where(d => d.Name == document.Name).Any())
            {
                document.Operation = (int)OperationType.Delete;
                isUpdated = true;
            }
        }

        return isUpdated;
    }
}