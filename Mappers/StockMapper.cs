using TucaAPI.Dtos.Stock;
using TucaAPI.Models;

namespace TucaAPI.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stockModel) {
            return new StockDto {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                Purchase = stockModel.Purchase,
                MarketCap = stockModel.MarketCap,
            };
        }
    }
}