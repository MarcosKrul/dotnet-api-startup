using Microsoft.EntityFrameworkCore;
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
            await this.context.AddAsync(stock);
            await this.context.SaveChangesAsync();
            return stock;
        }

        public async Task DeleteAsync(Stock stock)
        {
            this.context.Remove(stock);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await this.context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await this.context.Stocks.FindAsync(id);
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