using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Portfolio
{
    public class GetStocksFromUserPortfolioService
        : IService<UserAuthenticatedInfos, SuccessApiResponse<List<Stock>>>
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

        public async Task<SuccessApiResponse<List<Stock>>> ExecuteAsync(UserAuthenticatedInfos data)
        {
            var hasUser = await this.userManager.FindByEmailAsync(
                data.User.GetEmail().GetNonNullable()
            );
            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.USER_NOT_FOUND
                );

            var userPortFolio = await this.portfolioRepository.GetStocksFromUserPortfolio(hasUser);

            return new SuccessApiResponse<List<Stock>> { Content = userPortFolio };
        }
    }
}
