using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySystem.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class ValidateIdFilter(string idKey = "id") : Attribute, IAsyncActionFilter
{
    private readonly string _idKey = idKey;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

        if (
            context.RouteData.Values.TryGetValue(_idKey, out object? idObject)
            && idObject is string idString
            && int.TryParse(idString, out int id)
        )
        {
            if (id <= 0)
            {
                ApiResponse<object?> response = new();
                mapper.Map(ResponseStatus.IdOutOfRange, response);

                context.Result = new BadRequestObjectResult(response);
                return;
            }
        }
        await next();
    }
}
