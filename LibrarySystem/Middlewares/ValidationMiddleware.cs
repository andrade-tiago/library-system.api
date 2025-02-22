using LibrarySystem.DTOs.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace LibrarySystem.Middlewares;

public class ValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (
            context.Response.StatusCode == StatusCodes.Status400BadRequest
            && context.Items.TryGetValue("ModelState", out var modelStateObj)
            && modelStateObj is ModelStateDictionary modelState
        ) {
            var response = new ApiResponse<object>
            {
                Message = "Erro de validação",
                Errors  = modelState
                    .Where(ms => ms.Value!.Errors.Count > 0)
                    .ToDictionary(
                        ms => ms.Key,
                        ms => ms.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                    ),
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

