using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Portfolio;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Repositories;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Portfolio
{
    public class DeleteUserPortfolioService : IService<DeleteUserPortfolioRequestDto, ApiResponse>
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

        public async Task<ApiResponse> ExecuteAsync(DeleteUserPortfolioRequestDto data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            var userPortfolio = await this.portfolioRepository.GetUserPortfolio(data.StockId, user);

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
