using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
using TucaAPI.Extensions;
using TucaAPI.Models;
using TucaAPI.Repositories;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Extensions;
using TucaAPI.src.Services.Portfolio;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository stockRepository;
        private readonly IPortfolioRepository portfolioRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IServiceProvider serviceProvider;

        public PortfolioController(
            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository,
            IServiceProvider serviceProvider
        )
        {
            this.stockRepository = stockRepository;
            this.userManager = userManager;
            this.portfolioRepository = portfolioRepository;
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
            var hasUser = await this.userManager.FindByEmailAsync(email.GetNonNullable());
            if (hasUser is null)
                return Unauthorized(new ErrorApiResponse(MessageKey.USER_NOT_FOUND));

            var stock = await this.stockRepository.GetByIdAsync(stockId);

            if (stock is null)
                return BadRequest(new ErrorApiResponse(MessageKey.STOCK_NOT_FOUND));

            var userPortfolio = await this.portfolioRepository.GetStocksFromUserPortfolio(hasUser);

            if (userPortfolio.Any(i => i.Id == stock.Id))
                return BadRequest(new ErrorApiResponse(MessageKey.CANNOT_ADD_SAME_STOCK));

            var portfolioModel = new Portfolio { StockId = stock.Id, AppUserId = hasUser.Id };

            await this.portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel is null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ErrorApiResponse(MessageKey.COULD_NOT_CREATE)
                );

            return StatusCode(StatusCodes.Status201Created, new ApiResponse { Success = true });
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int stockId)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email.GetNonNullable());
            if (hasUser is null)
                return Unauthorized(new ErrorApiResponse(MessageKey.USER_NOT_FOUND));

            var userPortfolio = await this.portfolioRepository.GetUserPortfolio(stockId, hasUser);

            if (userPortfolio is null)
                return BadRequest(new ErrorApiResponse(MessageKey.STOCK_NOT_IN_PORTFOLIO));

            await this.portfolioRepository.DeleteAsync(userPortfolio);

            return Ok(new ApiResponse { Success = true });
        }
    }
}
