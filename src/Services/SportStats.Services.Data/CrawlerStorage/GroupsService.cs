namespace SportStats.Services.Data.CrawlerStorage;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SportStats.Data.Contexts;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public class GroupsService : BaseCrawlerStorageService, IGroupsService
{
    private readonly ILogger<GroupsService> logger;
    private readonly IZipService zipService;
    private readonly IMD5Hash hasher;
    private readonly ILogsService logsService;

    public GroupsService(CrawlerStorageDbContext context, ILogger<GroupsService> logger, IZipService zipService, IMD5Hash hasher, ILogsService logsService)
        : base(context)
    {
        this.logger = logger;
        this.zipService = zipService;
        this.hasher = hasher;
        this.logsService = logsService;
    }

    public async Task AddGroupAsync(Group group)
    {
        await this.Context.AddAsync(group);
        await this.Context.SaveChangesAsync();
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
        var group = await this.Context
            .Groups
            .FirstOrDefaultAsync(g => g.CrawlerId == crawlerId && g.Name == name);

        return group;
    }

    public async Task UpdateGroupAsync(Group newGroup, Group oldGroup)
    {
        var group = await this.Context
            .Groups
            .SingleAsync(g => g.Identifier == oldGroup.Identifier);

        await this.Context
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

        await this.Context.SaveChangesAsync();
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