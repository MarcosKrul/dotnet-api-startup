using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.TwoFactorAuthentication;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Providers.GoogleAuthenticator;
using TucaAPI.src.Services.Account;

namespace TucaAPI.src.Services.TwoFactorAuthentication
{
    public class LoginGoogleAuthenticator2FAService
        : LoginService<LoginGoogleAuthenticator2FARequestDto>
    {
        private readonly IGoogleAuthenticatorProvider googleAuthenticatorProvider;

        public LoginGoogleAuthenticator2FAService(
            IGoogleAuthenticatorProvider googleAuthenticatorProvider,
            UserManager<AppUser> userManager,
            ITokenProvider tokenProvider,
            SignInManager<AppUser> signInManager
        )
            : base(userManager, tokenProvider, signInManager)
        {
            this.googleAuthenticatorProvider = googleAuthenticatorProvider;
        }

        protected override void VerifyTwoFactorAuthentication(
            AppUser user,
            LoginGoogleAuthenticator2FARequestDto data
        )
        {
            if (!user.TwoFactorEnabled)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.TWO_FACTOR_DISABLED
                );

            if (
                !this.googleAuthenticatorProvider.ValidatePin(
                    user.TwoFactorSecret.GetNonNullable(),
                    data.Pin.GetNonNullable()
                )
            )
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.TWO_FACTOR_INVALID_CREDENTIALS
                );
        }
    }
}
