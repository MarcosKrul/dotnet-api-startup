using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Dtos.Account;
using TucaAPI.Models;
using TucaAPI.Providers;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Mappers;
using TucaAPI.Src.Services;

namespace TucaAPI.src.Services.Account
{
    public class ConfirmAccountService
        : IService<ConfirmDto, SuccessApiResponse<AuthenticatedUserDto>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenProvider tokenProvider;

        public ConfirmAccountService(UserManager<AppUser> userManager, ITokenProvider tokenProvider)
        {
            this.userManager = userManager;
            this.tokenProvider = tokenProvider;
        }

        public async Task<SuccessApiResponse<AuthenticatedUserDto>> ExecuteAsync(ConfirmDto data)
        {
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.USER_NOT_FOUND
                );

            var result = await this.userManager.ConfirmEmailAsync(
                hasUser,
                data.Token.GetNonNullable()
            );

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    result.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            var token = await this.tokenProvider.CreateAsync(hasUser);

            return new SuccessApiResponse<AuthenticatedUserDto>
            {
                Content = new AuthenticatedUserDto
                {
                    UserName = hasUser.UserName.GetNonNullable(),
                    Email = hasUser.Email.GetNonNullable(),
                    Token = token
                }
            };
        }
    }
}
