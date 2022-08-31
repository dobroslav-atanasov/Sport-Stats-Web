namespace SportStats.Web.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportStats.Web.Data;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext dbContext;

    public UsersService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<string>> GetUsernamesAsync()
    {
        var usernames = await this.dbContext
            .Users
            .Select(x => x.UserName)
            .ToListAsync();

        return usernames;
    }

    public async Task<int> TotalUsersAsync()
    {
        return await this.dbContext.Users.CountAsync();
    }
}
