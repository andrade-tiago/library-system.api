using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.HttpContext.Items["ModelState"] = context.ModelState;
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
