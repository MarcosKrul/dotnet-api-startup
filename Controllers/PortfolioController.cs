using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;

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
            var user = await this.userManager.FindByEmailAsync(email);

            var userPortFolio = await this.portfolioRepository.GetUserPortfolio(user);
    
            return Ok(userPortFolio);
        }
    }
}