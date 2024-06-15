using Newtonsoft.Json;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;

namespace TucaAPI.src.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
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
            catch
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    new ErrorApiResponse<string>
                    {
                        Errors = [MessageKey.INTERNAL_SERVER_ERROR]
                    });
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