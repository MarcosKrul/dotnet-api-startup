using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Portfolio
{
    public class AddStockAtUserPortfolioRequestDto : UserAuthenticatedInfos
    {
        public int StockId { get; set; }
    }
}
