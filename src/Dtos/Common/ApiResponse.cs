
namespace TucaAPI.src.Dtos.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
    }

    public class SuccessApiResponse<T> : ApiResponse
    {
        public T? Content { get; set; }

        public SuccessApiResponse()
        {
            this.Success = true;
        }
    }

    public class ErrorApiResponse : ApiResponse
    {
        public IEnumerable<AppErrorDescriptor>? Errors { get; set; }

        public ErrorApiResponse()
        {
            this.Success = false;
        }

        public ErrorApiResponse(string key)
        {
            this.Success = false;
            this.Errors = [new AppErrorDescriptor { Key = key }];
        }
    }
}