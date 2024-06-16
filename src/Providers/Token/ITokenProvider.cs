using TucaAPI.Models;

namespace TucaAPI.Providers
{
    public interface ITokenProvider
    {
        Task<string> CreateAsync(AppUser user);
    }
}
