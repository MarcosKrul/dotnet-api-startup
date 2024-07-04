using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.TwoFactorAuthentication;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers.GoogleAuthenticator;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.TwoFactorAuthentication
{
    public class ConfirmDisableGoogleAuthenticator2FAService
        : IService<ConfirmDisableGoogleAuthenticator2FARequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IGoogleAuthenticatorProvider googleAuthenticatorProvider;

        public ConfirmDisableGoogleAuthenticator2FAService(
            UserManager<AppUser> userManager,
            IGoogleAuthenticatorProvider googleAuthenticatorProvider
        )
        {
            this.userManager = userManager;
            this.googleAuthenticatorProvider = googleAuthenticatorProvider;
        }

        public async Task<ApiResponse> ExecuteAsync(
            ConfirmDisableGoogleAuthenticator2FARequestDto data
        )
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            if (!user.TwoFactorEnabled)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    MessageKey.TWO_FACTOR_REQUIRED
                );

            var tokenIsValid = await this.userManager.VerifyUserTokenAsync(
                user,
                "Default",
                "Disable2FA",
                data.Token.GetNonNullable()
            );

            if (!tokenIsValid)
                throw new AppException(StatusCodes.Status401Unauthorized, MessageKey.INVALID_TOKEN);

            var pinValid = this.googleAuthenticatorProvider.ValidatePin(
                user.TwoFactorSecret.GetNonNullable(),
                data.Pin.GetNonNullable()
            );

            if (!pinValid)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.INVALID_CREDENTIALS
                );

            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = null;
            var result = await this.userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status500InternalServerError,
                    MessageKey.ERROR_UPDATE_USER
                );

            return new ApiResponse { Success = true };
        }
    }
}
