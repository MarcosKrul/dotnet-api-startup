using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
using TucaAPI.Interfaces;
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

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await this.context.Portfolios
                .Where(i => i.AppUserId == user.Id)
                .Select(item => new Stock
                {
                    Id = item.StockId,
                    Symbol = item.Stock.Symbol,
                    CompanyName = item.Stock.CompanyName,
                    Purchase = item.Stock.Purchase,
                    LastDiv = item.Stock.LastDiv,
                    Industry = item.Stock.Industry,
                    MarketCap = item.Stock.MarketCap
                })
                .ToListAsync();
        }
    }
}