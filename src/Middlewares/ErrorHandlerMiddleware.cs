using Newtonsoft.Json;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;

namespace TucaAPI.src.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (AppException ex)
            {
                await HandleExceptionAsync(context, ex.StatusCode, ex.Error);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    new ErrorApiResponse(MessageKey.INTERNAL_SERVER_ERROR)
                );
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, object? value)
        {
            var result = JsonConvert.SerializeObject(value);
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
