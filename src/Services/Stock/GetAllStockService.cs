using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Mappers;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class GetAllStockService : IService<QueryStockDto, SuccessApiResponse<List<StockDto>>>
    {
        private readonly IStockRepository repository;

        public GetAllStockService(IStockRepository repository)
        {
            this.repository = repository;
        }

        public async Task<SuccessApiResponse<List<StockDto>>> ExecuteAsync(QueryStockDto data)
        {
            var stocks = await this.repository.GetAllAsync(data);
            var formatted = stocks.Select(i => i.ToStockDto()).ToList();

            return new SuccessApiResponse<List<StockDto>> { Content = formatted };
        }
    }
}
