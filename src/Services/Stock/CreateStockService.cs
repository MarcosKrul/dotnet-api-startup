using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Mappers;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class CreateStockService : IService<CreateStockRequestDto, SuccessApiResponse<StockDto>>
    {
        private readonly IStockRepository repository;

        public CreateStockService(IStockRepository repository)
        {
            this.repository = repository;
        }

        public async Task<SuccessApiResponse<StockDto>> ExecuteAsync(CreateStockRequestDto data)
        {
            var stock = data.ToStockFromCreateDTO();

            await this.repository.CreateAsync(stock);

            var newStock = await new GetStockByIdService(this.repository).ExecuteAsync(
                new GetStockByIdRequestDto { Id = stock.Id }
            );

            return newStock;
        }
    }
}
