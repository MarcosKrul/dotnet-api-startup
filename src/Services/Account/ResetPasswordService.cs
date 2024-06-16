using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Models;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Mappers;
using TucaAPI.Src.Services;

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
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.INVALID_CREDENTIALS
                );

            var result = await this.userManager.ResetPasswordAsync(
                hasUser,
                data.Token,
                data.Password
            );

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            return new ApiResponse { Success = true };
        }
    }
}
