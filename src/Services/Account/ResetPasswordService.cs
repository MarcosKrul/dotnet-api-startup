using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Models;
using TucaAPI.src.Repositories;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Account
{
    public class ResetPasswordService : IService<ResetPasswordRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordHistoryRepository passwordHistoryRepository;

        public ResetPasswordService(
            UserManager<AppUser> userManager,
            IPasswordHistoryRepository passwordHistoryRepository
        )
        {
            this.userManager = userManager;
            this.passwordHistoryRepository = passwordHistoryRepository;
        }

        public async Task<ApiResponse> ExecuteAsync(ResetPasswordRequestDto data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.Email.GetNonNullable(),
                MessageKey.INVALID_CREDENTIALS
            );

            var oldPasswordHash = user.PasswordHash.GetNonNullable();

            var result = await this.userManager.ResetPasswordAsync(user, data.Token, data.Password);

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            await this.passwordHistoryRepository.CreateAsync(
                new PasswordHistory { AppUserId = user.Id, Password = oldPasswordHash }
            );

            return new ApiResponse { Success = true };
        }
    }
}
