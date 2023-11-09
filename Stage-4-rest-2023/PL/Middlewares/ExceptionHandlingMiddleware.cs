using System.Net;
using Stage4rest2023.Exceptions;
using Stage4rest2023.Models.Responses;

namespace Stage4rest2023.Services;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorResponse
        {
            Success = false
        };

        switch (exception)
        {
            case ApplicationException:
            case InputValidationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest; //400
                errorResponse.Message = exception.Message;
                break;
            case TokenValidationException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized; //401
                errorResponse.Message = exception.Message;
                break;
            case InvalidCredentialsException:
                response.StatusCode = (int)HttpStatusCode.Forbidden; //403
                errorResponse.Message = exception.Message;
                break;
            case ItemNotFoundException _:
                response.StatusCode = (int)HttpStatusCode.NotFound; //404
                errorResponse.Message = exception.Message;
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
                errorResponse.Message = "Interne serverfout.";
                break;
        }

        _logger.LogError(exception.Message);
        await context.Response.WriteAsync(errorResponse.Message);
    }
}