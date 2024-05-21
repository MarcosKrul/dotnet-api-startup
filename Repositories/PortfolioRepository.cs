using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
using TucaAPI.Interfaces;
using TucaAPI.Models;

namespace TucaAPI.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext context;

        public CommentRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user) 
        {
            return await this.context.PortFolios
                .Where(i => i.AppUserId == user.Id)
                .Select(item => new Stock {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                })
                .ToListAsync();
        }
    }
}