using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
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
    }
}