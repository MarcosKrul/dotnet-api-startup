using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Repositories;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Portfolio
{
    public class GetStocksFromUserPortfolioService
        : IService<UserAuthenticatedInfos, SuccessApiResponse<List<Models.Stock>>>
    {
        private readonly IPortfolioRepository portfolioRepository;
        private readonly UserManager<AppUser> userManager;

        public GetStocksFromUserPortfolioService(
            UserManager<AppUser> userManager,
            IPortfolioRepository portfolioRepository
        )
        {
            this.userManager = userManager;
            this.portfolioRepository = portfolioRepository;
        }

        public async Task<SuccessApiResponse<List<Models.Stock>>> ExecuteAsync(
            UserAuthenticatedInfos data
        )
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            var userPortFolio = await this.portfolioRepository.GetStocksFromUserPortfolio(user);

            return new SuccessApiResponse<List<Models.Stock>> { Content = userPortFolio };
        }
    }
}
