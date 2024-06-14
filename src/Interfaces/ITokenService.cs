using TucaAPI.Models;

namespace TucaAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateAsync(AppUser user);
    }
}