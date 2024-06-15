using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; }
        public ErrorApiResponse<string>? Error { get; set; }

        public AppException(int statusCode, ErrorApiResponse<string>? error)
        {
            this.StatusCode = statusCode;
            this.Error = error;
        }
    }
}