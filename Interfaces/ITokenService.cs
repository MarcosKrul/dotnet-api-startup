using TucaAPI.Models;

namespace TucaAPI.Interfaces
{
    public interface ITokenService
    {
        string Create(AppUser user);
    }
}