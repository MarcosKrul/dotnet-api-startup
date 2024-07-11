using TucaAPI.src.Models;

namespace TucaAPI.src.Repositories
{
    public interface IPasswordHistoryRepository
    {
        Task<PasswordHistory> CreateAsync(PasswordHistory passwordHistory);
        Task<IEnumerable<string>> GetUserPasswordHistoryAsync(string userId, DateTime limit);
        Task<PasswordHistory?> GetMostRecentPasswordHistoryAsync(string userId, DateTime limit);
    }
}
