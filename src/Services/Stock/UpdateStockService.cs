using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class UpdateStockService
    {
        private readonly IStockRepository repository;

        public UpdateStockService(IStockRepository repository)
        {
            this.repository = repository;
        }
    }
}
