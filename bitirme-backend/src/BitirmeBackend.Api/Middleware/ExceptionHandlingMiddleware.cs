using System.Net;
using System.Text.Json;
using BitirmeBackend.Application.Exceptions;
using BitirmeBackend.Contracts.Common;
using Polly.CircuitBreaker;

namespace BitirmeBackend.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = ex switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
            ForbiddenAccessException => (HttpStatusCode.Forbidden, ex.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            BrokenCircuitException => (HttpStatusCode.ServiceUnavailable, "ML servisi şu an kullanılamıyor"),
            InvalidOperationException invalidOperation => MapInvalidOperation(invalidOperation),
            _ => (HttpStatusCode.InternalServerError, "Bir hata oluştu")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(ex, "Unhandled exception");
        else
            _logger.LogWarning(ex, "Handled exception: {Type}", ex.GetType().Name);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(json);
    }

    private static (HttpStatusCode StatusCode, string Message) MapInvalidOperation(InvalidOperationException ex)
    {
        if (ex.Message.StartsWith("ML servisi", StringComparison.OrdinalIgnoreCase))
        {
            if (ex.Message.Contains("haz", StringComparison.OrdinalIgnoreCase))
                return (HttpStatusCode.ServiceUnavailable, ex.Message);

            return (HttpStatusCode.BadGateway, ex.Message);
        }

        return (HttpStatusCode.InternalServerError, ex.Message);
    }
}
