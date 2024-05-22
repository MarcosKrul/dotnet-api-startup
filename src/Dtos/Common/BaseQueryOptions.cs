using System.ComponentModel.DataAnnotations;
using TucaAPI.Common;

namespace TucaAPI.Dtos.Common
{
    public class BaseQueryOptions
    {
        [Range(1, Constants.DEFAULT_PAGE_LIMIT)]
        public int Limit { get; set; } = Constants.DEFAULT_PAGE_LIMIT;
        [Range(0, int.MaxValue, ErrorMessage = "Only positive page number allowed")]
        public int Page { get; set; } = 0;
        public string? SortBy { get; set; } = null;
        public bool Asc { get; set; } = true;
    }
}