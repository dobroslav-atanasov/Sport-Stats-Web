namespace SportStats.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class MyExceptionFIlterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
    }
}
