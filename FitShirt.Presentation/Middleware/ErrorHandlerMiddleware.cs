using FitShirt.Presentation.Errors;
using Newtonsoft.Json;

namespace FitShirt.Presentation.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var codeErrorResponse = CodeErrorResponseFactory.CreateCodeErrorResponse(exception);

        var result = JsonConvert.SerializeObject(codeErrorResponse);

        context.Response.StatusCode = codeErrorResponse.StatusCode;

        await context.Response.WriteAsync(result);
    }
}