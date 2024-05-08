using Microsoft.AspNetCore.Mvc;
using TucaAPI.Data;
using TucaAPI.Dtos.Stock;
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
        public IActionResult GetAll()
        {
            var stocks = this.context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = this.context.Stocks.Find(id);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto data)
        {
            var stockModel = data.ToStockFromCreateDTO();

            this.context.Add(stockModel);
            this.context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto data)
        {
            var stock = this.context.Stocks.FirstOrDefault(x => x.Id == id);

            if (stock == null) return NotFound();

            stock.Symbol = data.Symbol;
            stock.CompanyName = data.CompanyName;
            stock.Industry = data.Industry;
            stock.LastDiv = data.LastDiv;
            stock.Purchase = data.Purchase;
            stock.MarketCap = data.MarketCap;

            this.context.SaveChanges();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = this.context.Stocks.FirstOrDefault(x => x.Id == id);

            if (stock == null) return NotFound();

            this.context.Remove(stock);
            this.context.SaveChanges();

            return NoContent();
        }
    }
}
