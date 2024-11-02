using System.Net;
using FitShirt.Application.Shared.Exceptions;

namespace FitShirt.Presentation.Errors;

public static class CodeErrorResponseFactory
{
    public static CodeErrorResponse CreateCodeErrorResponse(Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            ValidationException => (int)HttpStatusCode.BadRequest,
            ConflictException => (int)HttpStatusCode.Conflict,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return new CodeErrorResponse(statusCode, exception.Message);
    }
}