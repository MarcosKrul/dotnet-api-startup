using Microsoft.AspNetCore.Mvc;
using TucaAPI.Data;
using TucaAPI.Mappers;

namespace TucaAPI.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        
        public StockController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var stocks = this.context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            var stock = this.context.Stocks.Find(id);

            if (stock == null) return NotFound();
            
            return Ok(stock.ToStockDto());
        }
    }
}
