namespace SportStats.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class MyResultFilterAttribute : Attribute, IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
