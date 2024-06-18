using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Stock
{
    public class GetStockByIdService
    {
        private readonly IStockRepository repository;

        public GetStockByIdService(IStockRepository repository)
        {
            this.repository = repository;
        }
    }
}
