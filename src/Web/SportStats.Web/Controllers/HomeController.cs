namespace SportStats.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

using SportStats.Web.Filters;
using SportStats.Web.Services;
using SportStats.Web.ViewModels;

public class HomeController : Controller
{
    private readonly IUsersService usersService;

    public HomeController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [MyAuthorizationFilter]
    [MyResourceFilterAttirbute]
    [MyActionFIlter]
    [MyExceptionFIlter]
    [MyResultFilter]
    public async Task<IActionResult> Index()
    {
        var count = await this.usersService.TotalUsersAsync();
        var model = new UserViewModel { Count = count };
        return this.View(model);
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return this.View(
    //        new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    //}
}
