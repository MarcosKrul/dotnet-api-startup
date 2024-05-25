using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
using TucaAPI.Common;
using TucaAPI.Extensions;
using TucaAPI.Interfaces;
using TucaAPI.Models;
using TucaAPI.src.Common;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository stockRepository;
        private readonly IPortfolioRepository portfolioRepository;
        private readonly UserManager<AppUser> userManager;

        public PortfolioController(
            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository
        )
        {
            this.stockRepository = stockRepository;
            this.userManager = userManager;
            this.portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetStocksFromUserPortfolio()
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser == null) return Unauthorized();

            var userPortFolio = await this.portfolioRepository.GetStocksFromUserPortfolio(hasUser);

            return Ok(userPortFolio);
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser == null)
                return Unauthorized();

            var stock = await this.stockRepository.FindBySymbolAsync(symbol);

            if (stock == null)
                return BadRequest(Messages.STOCK_NOT_FOUND);

            var userPortfolio = await this.portfolioRepository.GetStocksFromUserPortfolio(hasUser);

            if (userPortfolio.Any(i => i.Id == stock.Id))
                return BadRequest(Messages.CANNOT_ADD_SAME_STOCK);

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = hasUser.Id
            };

            await this.portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel == null)
                return StatusCode(HttpStatus.INTERNAL_ERROR, Messages.COULD_NOT_CREATE);

            return Created();
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int stockId)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser == null)
                return Unauthorized();

            var userPortfolio = await this.portfolioRepository.GetUserPortfolio(stockId, hasUser);

            if (userPortfolio == null)
                return BadRequest(Messages.STOCK_NOT_IN_PORTFOLIO);

            await this.portfolioRepository.DeleteAsync(userPortfolio);

            return Ok();
        }
    }
}