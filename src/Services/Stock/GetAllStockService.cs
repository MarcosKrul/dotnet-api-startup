using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class GetAllStockService
    {
        private readonly IStockRepository repository;

        public GetAllStockService(IStockRepository repository)
        {
            this.repository = repository;
        }
    }
}
