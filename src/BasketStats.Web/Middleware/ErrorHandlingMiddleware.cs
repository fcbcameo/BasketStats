// src/BasketStats.Web/Middleware/ErrorHandlingMiddleware.cs
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace BasketStats.Web.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        object? errors = null;

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest; // 400
                errors = validationException.Errors.GroupBy(x => x.PropertyName)
                                             .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage));
                break;
                // Add other custom exception types here if needed
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (errors == null)
        {
            errors = new { error = exception.Message };
        }

        var result = JsonSerializer.Serialize(new { errors });
        return context.Response.WriteAsync(result);
    }
}