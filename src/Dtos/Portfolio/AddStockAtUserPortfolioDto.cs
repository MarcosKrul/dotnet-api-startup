using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Portfolio
{
    public class AddStockAtUserPortfolioDto : UserAuthenticatedInfos
    {
        public int StockId { get; set; }
    }
}
