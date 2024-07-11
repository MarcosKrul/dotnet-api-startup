using Microsoft.EntityFrameworkCore;
using TucaAPI.src.Data;
using TucaAPI.src.Models;

namespace TucaAPI.src.Repositories
{
    public class PasswordHistoryRepository : IPasswordHistoryRepository
    {
        private readonly ApplicationDBContext context;

        public PasswordHistoryRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<PasswordHistory> CreateAsync(PasswordHistory passwordHistory)
        {
            await this.context.PasswordHistories.AddAsync(passwordHistory);
            await this.context.SaveChangesAsync();
            return passwordHistory;
        }

        public async Task<PasswordHistory?> GetMostRecentPasswordHistoryAsync(
            string userId,
            DateTime limit
        )
        {
            return await this
                .context.PasswordHistories.Where(i => i.AppUserId == userId && i.UpdatedOn > limit)
                .OrderByDescending(i => i.UpdatedOn)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<string>> GetUserPasswordHistoryAsync(
            string userId,
            DateTime limit
        )
        {
            return await this
                .context.PasswordHistories.Where(i => i.AppUserId == userId && i.UpdatedOn > limit)
                .Select(i => i.Password)
                .ToListAsync();
        }
    }
}
