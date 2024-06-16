using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Stock
{
    public class QueryStockDto : BaseQueryOptions
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
    }
}
