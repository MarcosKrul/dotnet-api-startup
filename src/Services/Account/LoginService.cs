using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Account
{
    public class LoginService : IService<LoginRequestDto, SuccessApiResponse<AuthenticatedUserDto>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenProvider tokenProvider;
        private readonly SignInManager<AppUser> signInManager;

        public LoginService(
            UserManager<AppUser> userManager,
            ITokenProvider tokenProvider,
            SignInManager<AppUser> signInManager
        )
        {
            this.tokenProvider = tokenProvider;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<SuccessApiResponse<AuthenticatedUserDto>> ExecuteAsync(
            LoginRequestDto data
        )
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.Email.GetNonNullable(),
                MessageKey.INVALID_CREDENTIALS
            );

            var result = await this.signInManager.CheckPasswordSignInAsync(
                user,
                data.Password.GetNonNullable(),
                true
            );

            if (result.IsLockedOut)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.ACCOUNT_LOCKED
                );

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.INVALID_CREDENTIALS
                );
            ;

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
