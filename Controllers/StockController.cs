using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
using TucaAPI.Dtos.Stock;
using TucaAPI.Interfaces;
using TucaAPI.Mappers;

namespace TucaAPI.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository repository;

        public StockController(IStockRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await this.repository.GetAllAsync();
            var formatted = stocks.Select(s => s.ToStockDto());
            
            return Ok(formatted);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await this.repository.GetByIdAsync(id);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto data)
        {
            var stock = data.ToStockFromCreateDTO();

            await this.repository.CreateAsync(stock);

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto data)
        {
            var stock = await this.repository.UpdateAsync(id, data);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await this.repository.GetByIdAsync(id);

            if (stock == null) return NotFound();

            await this.repository.DeleteAsync(stock);

            return NoContent();
        }
    }
}
