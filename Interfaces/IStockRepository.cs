using TucaAPI.Dtos.Stock;
using TucaAPI.Models;

namespace TucaAPI.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task DeleteAsync(Stock stock);
    }
}