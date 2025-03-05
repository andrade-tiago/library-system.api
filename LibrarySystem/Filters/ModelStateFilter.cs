using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySystem.Filters;

public class ModelStateFilter(IMapper mapper) : IAsyncActionFilter
{
    private readonly IMapper _mapper = mapper;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            ApiResponse<object> response = new();
            _mapper.Map(ResponseStatus.ValidationErrors, response);

            response.Errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

            context.Result = new BadRequestObjectResult(response);
            return;
        }
        await next();
    }
}
