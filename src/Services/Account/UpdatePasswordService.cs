using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
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
    public class UpdatePasswordService : IService<UpdatePasswordRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordHistoryRepository passwordHistoryRepository;

        public UpdatePasswordService(
            UserManager<AppUser> userManager,
            IPasswordHistoryRepository passwordHistoryRepository
        )
        {
            this.userManager = userManager;
            this.passwordHistoryRepository = passwordHistoryRepository;
        }

        public async Task<ApiResponse> ExecuteAsync(UpdatePasswordRequestDto data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            if (data.NewPassword == data.OldPassword)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    MessageKey.UPDATE_TO_SAME_PASSWORD
                );

            var passwordHistory = await this.passwordHistoryRepository.GetUserPasswordHistory(
                user.Id,
                DateTime.Now.Add(Constants.TIME_TO_ENABLE_PASSWORD_USAGE)
            );

            if (
                passwordHistory.Any()
                && this.userManager.IsPasswordInHistory(
                    user,
                    data.NewPassword.GetNonNullable(),
                    passwordHistory
                )
            )
                throw new AppException(
                    StatusCodes.Status404NotFound,
                    MessageKey.PASSWORD_ALREADY_USED
                );

            var oldPasswordHash = user.PasswordHash.GetNonNullable();

            var result = await userManager.ChangePasswordAsync(
                user,
                data.OldPassword.GetNonNullable(),
                data.NewPassword.GetNonNullable()
            );

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
