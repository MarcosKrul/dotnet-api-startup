using TucaAPI.Dtos.Common;

namespace TucaAPI.Dtos.Stock
{
    public class QueryStockDto : BaseQueryOptions
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
    }
}