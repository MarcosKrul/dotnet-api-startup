using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Mappers;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Controllers
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
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetAll([FromQuery] QueryStockDto query)
        {
            var stocks = await this.repository.GetAllAsync(query);
            var formatted = stocks.Select(i => i.ToStockDto()).ToList();

            return Ok(new SuccessApiResponse<List<StockDto>> { Content = formatted });
        }

        [HttpGet("{id:int}")]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await this.repository.GetByIdAsync(id);

            if (stock is null)
                return NotFound(new ErrorApiResponse(MessageKey.STOCK_NOT_FOUND));

            return Ok(new SuccessApiResponse<StockDto> { Content = stock.ToStockDto() });
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto data)
        {
            var stock = data.ToStockFromCreateDTO();

            await this.repository.CreateAsync(stock);

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateStockRequestDto data
        )
        {
            var stock = await this.repository.UpdateAsync(id, data);

            if (stock is null)
                return NotFound(new ErrorApiResponse(MessageKey.STOCK_NOT_FOUND));

            return Ok(new SuccessApiResponse<StockDto> { Content = stock.ToStockDto() });
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await this.repository.GetByIdAsync(id);

            if (stock is null)
                return NotFound(new ErrorApiResponse(MessageKey.STOCK_NOT_FOUND));

            await this.repository.DeleteAsync(stock);

            return Ok(new ApiResponse { Success = true });
        }
    }
}
