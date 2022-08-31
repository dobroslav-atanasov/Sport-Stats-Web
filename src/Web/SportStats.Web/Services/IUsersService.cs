namespace SportStats.Web.Services;

public interface IUsersService
{
    Task<IEnumerable<string>> GetUsernamesAsync();

    Task<int> TotalUsersAsync();
}