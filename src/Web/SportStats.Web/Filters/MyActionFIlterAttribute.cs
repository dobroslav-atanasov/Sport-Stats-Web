namespace SportStats.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class MyActionFIlterAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
