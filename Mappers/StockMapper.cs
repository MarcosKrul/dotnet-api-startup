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
                Comments = stockModel.Comments.Select(item => item.ToCommentDto()).ToList(),
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto createStockRequestDto) {
            return new Stock {
                Symbol = createStockRequestDto.Symbol,
                CompanyName = createStockRequestDto.CompanyName,
                Industry = createStockRequestDto.Industry,
                LastDiv = createStockRequestDto.LastDiv,
                Purchase = createStockRequestDto.Purchase,
                MarketCap = createStockRequestDto.MarketCap,
            };
        }
    }
}