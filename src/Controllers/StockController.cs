using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Dtos.Stock;
using TucaAPI.src.Services.Stock;

namespace TucaAPI.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public StockController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetAll([FromQuery] QueryStockDto query)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<GetAllStockService>();
                var result = await service.ExecuteAsync(query);
                return Ok(result);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<GetStockByIdService>();
                var result = await service.ExecuteAsync(new GetStockByIdRequestDto { Id = id });
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<CreateStockService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPut]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Update([FromBody] UpdateStockRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<UpdateStockService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<DeleteStockService>();
                var result = await service.ExecuteAsync(new DeleteStockRequestDto { Id = id });
                return Ok(result);
            }
        }
    }
}
