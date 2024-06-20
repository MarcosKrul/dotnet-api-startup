using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Account
{
    public class ConfirmAccountService
        : IService<ConfirmRequestDto, SuccessApiResponse<AuthenticatedUserDto>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenProvider tokenProvider;

        public ConfirmAccountService(UserManager<AppUser> userManager, ITokenProvider tokenProvider)
        {
            this.userManager = userManager;
            this.tokenProvider = tokenProvider;
        }

        public async Task<SuccessApiResponse<AuthenticatedUserDto>> ExecuteAsync(
            ConfirmRequestDto data
        )
        {
            var user = await this.userManager.FindNonNullableUserAsync(data.Email.GetNonNullable());

            var result = await this.userManager.ConfirmEmailAsync(
                user,
                data.Token.GetNonNullable()
            );

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    result.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            var token = await this.tokenProvider.CreateAsync(user);

            return new SuccessApiResponse<AuthenticatedUserDto>
            {
                Content = new AuthenticatedUserDto
                {
                    UserName = user.UserName.GetNonNullable(),
                    Email = user.Email.GetNonNullable(),
                    Token = token
                }
            };
        }
    }
}
