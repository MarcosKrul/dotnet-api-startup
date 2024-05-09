using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetAll()
        {
            var stocks = await this.context.Stocks.ToListAsync();
            var formatted = stocks.Select(s => s.ToStockDto());
            
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await this.context.Stocks.FindAsync(id);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto data)
        {
            var stockModel = data.ToStockFromCreateDTO();

            await this.context.AddAsync(stockModel);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto data)
        {
            var stock = await this.context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null) return NotFound();

            stock.Symbol = data.Symbol;
            stock.CompanyName = data.CompanyName;
            stock.Industry = data.Industry;
            stock.LastDiv = data.LastDiv;
            stock.Purchase = data.Purchase;
            stock.MarketCap = data.MarketCap;

            await this.context.SaveChangesAsync();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await this.context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null) return NotFound();

            this.context.Remove(stock);
            await this.context.SaveChangesAsync();

            return NoContent();
        }
    }
}
