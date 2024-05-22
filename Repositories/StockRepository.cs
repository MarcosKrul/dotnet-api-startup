using Microsoft.EntityFrameworkCore;
using TucaAPI.Common;
using TucaAPI.Data;
using TucaAPI.Dtos.Stock;
using TucaAPI.Interfaces;
using TucaAPI.Models;

namespace TucaAPI.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext context;

        public StockRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await this.context.Stocks.AddAsync(stock);
            await this.context.SaveChangesAsync();
            return stock;
        }

        public async Task DeleteAsync(Stock stock)
        {
            this.context.Remove(stock);
            await this.context.SaveChangesAsync();
        }

        public async Task<Stock?> FindBySymbolAsync(string symbol)
        {
            return await this.context.Stocks.FirstOrDefaultAsync(item => item.Symbol == symbol);
        }

        public async Task<List<Stock>> GetAllAsync(QueryStockDto query)
        {
            var stockQuery = this.context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
                stockQuery = stockQuery.Where(item => item.CompanyName.Contains(query.CompanyName));

            if (!string.IsNullOrWhiteSpace(query.Symbol))
                stockQuery = stockQuery.Where(item => item.Symbol.Contains(query.Symbol));


            if (!string.IsNullOrWhiteSpace(query.SortBy) && query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stockQuery = query.Asc
                    ? stockQuery.OrderBy(item => item.Symbol)
                    : stockQuery.OrderByDescending(item => item.Symbol);


            return await stockQuery.Skip(query.Limit * query.Page).Take(query.Limit).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await this.context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<bool> StockExistsAsync(int id)
        {
            return this.context.Stocks.AnyAsync(item => item.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stock = await this.GetByIdAsync(id);

            if (stock == null) return null;

            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Industry = stockDto.Industry;
            stock.LastDiv = stockDto.LastDiv;
            stock.Purchase = stockDto.Purchase;
            stock.MarketCap = stockDto.MarketCap;

            await this.context.SaveChangesAsync();

            return stock;
        }
    }
}