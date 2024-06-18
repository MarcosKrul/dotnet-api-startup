using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class DeleteStockService : IService<DeleteStockRequestDto, ApiResponse>
    {
        private readonly IStockRepository repository;

        public DeleteStockService(IStockRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ApiResponse> ExecuteAsync(DeleteStockRequestDto data)
        {
            var stock = await this.repository.GetByIdAsync(data.Id);

            if (stock is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.STOCK_NOT_FOUND);

            await this.repository.DeleteAsync(stock);

            return new ApiResponse { Success = true };
        }
    }
}
