using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; }
        public ErrorApiResponse? Error { get; set; }

        public AppException(int statusCode, IEnumerable<AppErrorDescriptor> Errors)
        {
            this.StatusCode = statusCode;
            this.Error = new ErrorApiResponse { Errors = Errors };
        }

        public AppException(int statusCode, string key)
        {
            this.StatusCode = statusCode;
            this.Error = new ErrorApiResponse { Errors = [new AppErrorDescriptor { Key = key }] };
        }
    }
}