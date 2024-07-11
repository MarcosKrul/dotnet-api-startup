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
    }
}
