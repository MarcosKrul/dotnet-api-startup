
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

    public class ErrorApiResponse<T> : ApiResponse
    {
        public IEnumerable<T>? Errors { get; set; }

        public ErrorApiResponse()
        {
            this.Success = false;
        }
    }
}