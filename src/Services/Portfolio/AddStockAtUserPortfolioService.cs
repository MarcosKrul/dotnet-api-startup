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
    public class AddStockAtUserPortfolioService
        : IService<AddStockAtUserPortfolioRequestDto, ApiResponse>
    {
        private readonly IStockRepository stockRepository;
        private readonly IPortfolioRepository portfolioRepository;
        private readonly UserManager<AppUser> userManager;

        public AddStockAtUserPortfolioService(
            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository
        )
        {
            this.stockRepository = stockRepository;
            this.userManager = userManager;
            this.portfolioRepository = portfolioRepository;
        }

        public async Task<ApiResponse> ExecuteAsync(AddStockAtUserPortfolioRequestDto data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            var stock = await this.stockRepository.GetByIdAsync(data.StockId);

            if (stock is null)
                throw new AppException(StatusCodes.Status400BadRequest, MessageKey.STOCK_NOT_FOUND);

            var userPortfolio = await this.portfolioRepository.GetStocksFromUserPortfolio(user);

            if (userPortfolio.Any(i => i.Id == stock.Id))
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    MessageKey.CANNOT_ADD_SAME_STOCK
                );

            var portfolioModel = new Models.Portfolio { StockId = stock.Id, AppUserId = user.Id };

            await this.portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel is null)
                throw new AppException(
                    StatusCodes.Status500InternalServerError,
                    MessageKey.COULD_NOT_CREATE
                );

            return new ApiResponse { Success = true };
        }
    }
}
