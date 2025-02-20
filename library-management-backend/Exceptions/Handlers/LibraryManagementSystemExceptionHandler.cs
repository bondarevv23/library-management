using System.Net.Mime;
using System.Text.Json;

namespace LibraryManagementSystem.Exceptions.Handlers;

public class ApplicationExceptionHandler(RequestDelegate next, ILogger<ApplicationExceptionHandler> logger)
{
    private readonly RequestDelegate _next = next;

    private readonly ILogger<ApplicationExceptionHandler> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (LibraryManagementSystemException applicationException)
        {
            _logger.LogError(applicationException.LogDescription);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = applicationException.Code;

            var response = new
            {
                code = context.Response.StatusCode,
                message = applicationException.Message,
                timestamp = DateTime.Now
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception exception)
        {
            _logger.LogError("Unexpected exception: {}", exception);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                code = context.Response.StatusCode,
                message = "An unexpected error occurred.",
                timestamp = DateTime.Now
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
