namespace SportStats.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class MyAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
    }
}
