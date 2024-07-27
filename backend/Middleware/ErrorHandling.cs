using System.Text.Json;

namespace TodoApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
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
            context.Response.ContentType = "application/json";

            int statusCode = StatusCodes.Status500InternalServerError; // Default status code

            if (ex.Data["StatusCode"] != null)
            {
                statusCode = (int) ex.Data["StatusCode"]!;
            }

            context.Response.StatusCode = statusCode;

            var response = JsonSerializer.Serialize(new { error = new { detail = ex.Message, code = statusCode } });
            await context.Response.WriteAsync(response);
        }
    }
}
