using System.ComponentModel.DataAnnotations.Schema;

namespace TucaAPI.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public AppUser AppUser { get; set; }
    }
}
