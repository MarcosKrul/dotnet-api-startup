using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.TwoFactorAuthentication;

namespace TucaAPI.src.Services.TwoFactorAuthentication
{
    public class ConfirmDisableGoogleAuthenticator2FAService
        : IService<ConfirmDisableGoogleAuthenticator2FARequestDto, ApiResponse>
    {
        public Task<ApiResponse> ExecuteAsync(ConfirmDisableGoogleAuthenticator2FARequestDto data)
        {
            throw new NotImplementedException();
        }
    }
}
