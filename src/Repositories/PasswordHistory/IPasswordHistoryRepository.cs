using TucaAPI.src.Models;

namespace TucaAPI.src.Repositories
{
    public interface IPasswordHistoryRepository
    {
        Task<PasswordHistory> CreateAsync(PasswordHistory passwordHistory);
        Task<IEnumerable<string>> GetUserPasswordHistory(string userId, DateTime limit);
    }
}
