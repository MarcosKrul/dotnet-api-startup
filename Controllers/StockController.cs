using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
using TucaAPI.Dtos.Stock;
using TucaAPI.Interfaces;
using TucaAPI.Mappers;

namespace TucaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository repository;

        public StockController(IStockRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ValidateModelState]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await this.repository.GetAllAsync();
            var formatted = stocks.Select(s => s.ToStockDto());

            return Ok(formatted);
        }

        [HttpGet("{id:int}")]
        [ValidateModelState]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await this.repository.GetByIdAsync(id);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto data)
        {
            var stock = data.ToStockFromCreateDTO();

            await this.repository.CreateAsync(stock);

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto data)
        {
            var stock = await this.repository.UpdateAsync(id, data);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await this.repository.GetByIdAsync(id);

            if (stock == null) return NotFound();

            await this.repository.DeleteAsync(stock);

            return NoContent();
        }
    }
}
