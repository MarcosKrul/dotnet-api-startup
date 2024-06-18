using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class DeleteStockService
    {
        private readonly IStockRepository repository;

        public DeleteStockService(IStockRepository repository)
        {
            this.repository = repository;
        }
    }
}
