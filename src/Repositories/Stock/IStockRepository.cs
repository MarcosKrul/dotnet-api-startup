using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Models;

namespace TucaAPI.src.Repositories
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryStockDto query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(UpdateStockRequestDto stockDto);
        Task DeleteAsync(Stock stock);
        Task<bool> StockExistsAsync(int id);
    }
}
