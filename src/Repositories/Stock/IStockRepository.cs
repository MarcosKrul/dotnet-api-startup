using TucaAPI.Dtos.Stock;
using TucaAPI.Models;

namespace TucaAPI.Repositories
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryStockDto query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task DeleteAsync(Stock stock);
        Task<bool> StockExistsAsync(int id);
    }
}
