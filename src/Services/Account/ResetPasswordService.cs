using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Models;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Account
{
    public class ResetPasswordService : IService<ResetPasswordRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;

        public ResetPasswordService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ApiResponse> ExecuteAsync(ResetPasswordRequestDto data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.Email.GetNonNullable(),
                MessageKey.INVALID_CREDENTIALS
            );

            var result = await this.userManager.ResetPasswordAsync(user, data.Token, data.Password);

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            return new ApiResponse { Success = true };
        }
    }
}
