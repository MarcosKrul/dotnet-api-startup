using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Extensions;
using TucaAPI.src.Attributes;
using TucaAPI.src.Dtos.Portfolio;
using TucaAPI.src.Extensions;
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
            var email = User.GetEmail();
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<GetStocksFromUserPortfolioService>();
                var result = await service.ExecuteAsync(email.GetNonNullable());
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        [Route("{stockId:int}")]
        public async Task<IActionResult> AddPortfolio([FromRoute] int stockId)
        {
            var email = User.GetEmail();
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<AddStockAtUserPortfolioService>();
                var result = await service.ExecuteAsync(
                    new AddStockAtUserPortfolioDto { Email = email, StockId = stockId }
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
            var email = User.GetEmail();
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<DeleteUserPortfolioService>();
                var result = await service.ExecuteAsync(
                    new DeleteUserPortfolioDto { Email = email, StockId = stockId }
                );
                return Ok(result);
            }
        }
    }
}
