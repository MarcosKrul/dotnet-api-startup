using TucaAPI.src.Data;

namespace TucaAPI.src.Repositories.PasswordHistory.Implementation
{
    public class PasswordHistoryRepository : IPasswordHistoryRepository
    {
        private readonly ApplicationDBContext context;

        public PasswordHistoryRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
    }
}
