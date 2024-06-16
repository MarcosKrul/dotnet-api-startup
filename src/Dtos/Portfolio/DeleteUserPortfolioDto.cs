using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Portfolio
{
    public class DeleteUserPortfolioDto : UserAuthenticatedInfos
    {
        public int StockId { get; set; }
    }
}
