using TucaAPI.src.Models;

namespace TucaAPI.src.Providers
{
    public interface ITokenProvider
    {
        Task<string> CreateAsync(AppUser user);
    }
}
