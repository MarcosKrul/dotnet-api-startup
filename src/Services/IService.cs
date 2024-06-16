using TucaAPI.src.Dtos.Common;

namespace TucaAPI.Src.Services
{
    public interface IService<T, K>
        where K : ApiResponse
    {
        Task<K> ExecuteAsync(T data);
    }

    public interface IService<K>
        where K : ApiResponse
    {
        Task<K> ExecuteAsync();
    }
}
