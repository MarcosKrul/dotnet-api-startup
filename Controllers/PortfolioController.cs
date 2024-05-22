using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
using TucaAPI.Common;
using TucaAPI.Extensions;
using TucaAPI.Interfaces;
using TucaAPI.Models;

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
        public async Task<IActionResult> GetUserPortfolio()
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser == null) return Unauthorized();

            var userPortFolio = await this.portfolioRepository.GetUserPortfolio(hasUser);

            return Ok(userPortFolio);
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser == null) return Unauthorized();

            var stock = await this.stockRepository.FindBySymbolAsync(symbol);

            if (stock == null) return BadRequest("Stock not found");

            var userPortfolio = await this.portfolioRepository.GetUserPortfolio(hasUser);

            if (userPortfolio.Any(e => e.Id == stock.Id))
                return BadRequest("Cannot add same stock to portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = hasUser.Id
            };

            await this.portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel == null) return StatusCode(HttpStatus.INTERNAL_ERROR, "Could not create");

            return Created();
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Delete(string symbol)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser == null) return Unauthorized();

            var userPortfolio = await this.portfolioRepository.GetUserPortfolio(hasUser);

            var filteredStock = userPortfolio
                .Where(s => s.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            if (filteredStock.Count != 1) return BadRequest("Stock not in you portfolio");

            await this.portfolioRepository.DeleteAsync(new Portfolio
            {
                AppUserId = hasUser.Id,
                StockId = filteredStock.First().Id
            });

            return Ok();
        }
    }
}