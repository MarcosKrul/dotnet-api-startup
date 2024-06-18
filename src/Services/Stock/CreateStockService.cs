using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class CreateStockService
    {
        private readonly IStockRepository repository;

        public CreateStockService(IStockRepository repository)
        {
            this.repository = repository;
        }
    }
}
