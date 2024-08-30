using ReservationManagementSystem.API.Extensions;
using System.Net.Mime;
using System.Text.Json;

namespace ReservationManagementSystem.API.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        if (exception is ArgumentNullException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new CustomErrorResponse
            {
                Message = "One or more validation errors occurred.",
                Errors = new List<string> { exception.Message }
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var defaultResponse = new CustomErrorResponse
        {
            Message = "An unexpected error occurred.",
            Errors = new List<string> { exception.Message }
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(defaultResponse));
    }
}
