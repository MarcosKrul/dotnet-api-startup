using TucaAPI.Models;

namespace TucaAPI.Repositories
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetStocksFromUserPortfolio(AppUser user);
        Task<Portfolio?> GetUserPortfolio(int stockId, AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task DeleteAsync(Portfolio portfolio);
    }
}
