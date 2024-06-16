using TucaAPI.src.Dtos.Common;

namespace TucaAPI.Src.Services
{
    public interface IService<T, K> where T : ApiResponse
    {
        Task<T> ExecuteAsync(K data);
    }
}