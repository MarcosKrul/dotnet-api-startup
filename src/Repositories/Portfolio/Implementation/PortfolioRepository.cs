using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
using TucaAPI.Models;

namespace TucaAPI.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext context;

        public PortfolioRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await this.context.Portfolios.AddAsync(portfolio);
            await this.context.SaveChangesAsync();
            return portfolio;
        }

        public async Task DeleteAsync(Portfolio portfolio)
        {
            this.context.Portfolios.Remove(portfolio);
            await this.context.SaveChangesAsync();
        }

        public async Task<Portfolio?> GetUserPortfolio(int stockId, AppUser user)
        {
            return await this.context.Portfolios.FirstOrDefaultAsync(i =>
                i.StockId == stockId && i.AppUserId == user.Id
            );
        }

        public async Task<List<Stock>> GetStocksFromUserPortfolio(AppUser user)
        {
            return await this
                .context.Portfolios.Where(i => i.AppUserId == user.Id)
                .Select(i => new Stock
                {
                    Id = i.StockId,
                    Symbol = i.Stock.Symbol,
                    CompanyName = i.Stock.CompanyName,
                    Purchase = i.Stock.Purchase,
                    LastDiv = i.Stock.LastDiv,
                    Industry = i.Stock.Industry,
                    MarketCap = i.Stock.MarketCap
                })
                .ToListAsync();
        }
    }
}
