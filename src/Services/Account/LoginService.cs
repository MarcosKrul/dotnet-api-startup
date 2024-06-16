using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;

namespace TucaAPI.src.Services.Account
{
    public class LoginService : IService<LoginDto, SuccessApiResponse<AuthenticatedUserDto>>
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

        public async Task<SuccessApiResponse<AuthenticatedUserDto>> ExecuteAsync(LoginDto data)
        {
            var unauthorizedError = new AppException(
                StatusCodes.Status401Unauthorized,
                MessageKey.INVALID_CREDENTIALS
            );

            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                throw unauthorizedError;

            var result = await this.signInManager.CheckPasswordSignInAsync(
                hasUser,
                data.Password.GetNonNullable(),
                true
            );

            if (result.IsLockedOut)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.ACCOUNT_LOCKED
                );

            if (!result.Succeeded)
                throw unauthorizedError;

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
