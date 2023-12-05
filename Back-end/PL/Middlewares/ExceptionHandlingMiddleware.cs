using PL.Exceptions;
using PL.Models.Responses;
using System.Net;

namespace PL.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the next middleware in the pipeline and catches any exceptions that may occur.
        /// </summary>
        /// <param name="httpContext">The HttpContext for the current request.</param>
        /// <returns>
        /// Task representing the asynchronous execution of the middleware.
        /// </returns>
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

        /// <summary>
        /// Handles exceptions by setting the appropriate HTTP status code and returning an error response.
        /// </summary>
        /// <param name="context">The HttpContext for the current request.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>
        /// Task representing the asynchronous handling of the exception.
        /// </returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpResponse response = context.Response;

            ErrorResponse errorResponse = new ErrorResponse
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
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound; //404
                    errorResponse.Message = exception.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
                    errorResponse.Message = "Interne serverfout. - " + exception.Message;
                    break;
            }

            _logger.LogError(exception.Message);
            await context.Response.WriteAsync(errorResponse.Message);
        }
    }
}
