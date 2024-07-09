using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
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
    public class UpdatePasswordService : IService<UpdatePasswordRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;

        public UpdatePasswordService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
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

            return new ApiResponse { Success = true };
        }
    }
}
