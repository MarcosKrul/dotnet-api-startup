using TucaAPI.Extensions;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Extensions;
using TucaAPI.src.Providers.GoogleAuthenticator;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.TwoFactorAuthentication
{
    public class GetGoogleAuthenticatorQRCodeService
        : IService<UserAuthenticatedInfos, SuccessApiResponse<string>>
    {
        private readonly IGoogleAuthenticatorProvider googleAuthenticatorProvider;

        public GetGoogleAuthenticatorQRCodeService(
            IGoogleAuthenticatorProvider googleAuthenticatorProvider
        )
        {
            this.googleAuthenticatorProvider = googleAuthenticatorProvider;
        }

        public async Task<SuccessApiResponse<string>> ExecuteAsync(UserAuthenticatedInfos data)
        {
            var infos = this.googleAuthenticatorProvider.GetUserSetupInfos(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            return new SuccessApiResponse<string> { Content = infos.QrCodeImageUrl };
        }
    }
}
