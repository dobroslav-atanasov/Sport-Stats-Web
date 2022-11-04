namespace SportStats.Services.Data.CrawlerStorage.Interfaces;

using SportStats.Data.Models.Entities.Crawlers;

public interface IGroupsService
{
    Task AddOrUpdateGroupAsync(Group group);

    Task<Group> GetGroupAsync(int crawlerId, string name);

    Task AddGroupAsync(Group group);

    Task UpdateGroupAsync(Group newGroup, Group oldGroup);

    Task<IList<string>> GetGroupNamesAsync(int crawlerId);
}