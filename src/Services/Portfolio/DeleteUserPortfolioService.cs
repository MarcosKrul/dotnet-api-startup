using Microsoft.AspNetCore.Identity;
using TucaAPI.Models;
using TucaAPI.Repositories;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Portfolio;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.Src.Services;

namespace TucaAPI.src.Services.Portfolio
{
    public class DeleteUserPortfolioService : IService<DeleteUserPortfolioDto, ApiResponse>
    {
        private readonly IPortfolioRepository portfolioRepository;
        private readonly UserManager<AppUser> userManager;

        public DeleteUserPortfolioService(
            UserManager<AppUser> userManager,
            IPortfolioRepository portfolioRepository
        )
        {
            this.userManager = userManager;
            this.portfolioRepository = portfolioRepository;
        }

        public async Task<ApiResponse> ExecuteAsync(DeleteUserPortfolioDto data)
        {
            var hasUser = await this.userManager.FindByEmailAsync(data.Email.GetNonNullable());
            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.USER_NOT_FOUND
                );

            var userPortfolio = await this.portfolioRepository.GetUserPortfolio(
                data.StockId,
                hasUser
            );

            if (userPortfolio is null)
                throw new AppException(
                    StatusCodes.Status404NotFound,
                    MessageKey.STOCK_NOT_IN_PORTFOLIO
                );

            await this.portfolioRepository.DeleteAsync(userPortfolio);

            return new ApiResponse { Success = true };
        }
    }
}
