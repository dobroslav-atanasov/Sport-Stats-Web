namespace SportStats.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class MyResourceFilterAttirbute : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}