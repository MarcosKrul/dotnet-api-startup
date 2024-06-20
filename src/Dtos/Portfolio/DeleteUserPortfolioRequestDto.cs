using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Portfolio
{
    public class DeleteUserPortfolioRequestDto : UserAuthenticatedInfos
    {
        public int StockId { get; set; }
    }
}
