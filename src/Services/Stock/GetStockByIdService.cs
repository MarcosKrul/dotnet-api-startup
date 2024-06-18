using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class GetStockByIdService
        : IService<GetStockByIdRequestDto, SuccessApiResponse<StockDto>>
    {
        private readonly IStockRepository repository;

        public GetStockByIdService(IStockRepository repository)
        {
            this.repository = repository;
        }

        public async Task<SuccessApiResponse<StockDto>> ExecuteAsync(GetStockByIdRequestDto data)
        {
            var stock = await this.repository.GetByIdAsync(data.Id);

            if (stock is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.STOCK_NOT_FOUND);

            return new SuccessApiResponse<StockDto> { Content = stock.ToStockDto() };
        }
    }
}
