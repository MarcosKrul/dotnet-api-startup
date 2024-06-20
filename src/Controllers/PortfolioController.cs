using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Portfolio;
using TucaAPI.src.Services.Portfolio;

namespace TucaAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public PortfolioController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetStocksFromUserPortfolio()
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<GetStocksFromUserPortfolioService>();
                var result = await service.ExecuteAsync(new UserAuthenticatedInfos { User = User });
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        [Route("{stockId:int}")]
        public async Task<IActionResult> AddPortfolio([FromRoute] int stockId)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<AddStockAtUserPortfolioService>();
                var result = await service.ExecuteAsync(
                    new AddStockAtUserPortfolioRequestDto { User = User, StockId = stockId }
                );
                return Ok(result);
            }
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int stockId)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<DeleteUserPortfolioService>();
                var result = await service.ExecuteAsync(
                    new DeleteUserPortfolioRequestDto { User = User, StockId = stockId }
                );
                return Ok(result);
            }
        }
    }
}
