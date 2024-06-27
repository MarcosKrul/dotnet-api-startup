using TucaAPI.src.Dtos.TwoFactorAuthentication;

namespace TucaAPI.src.Providers.GoogleAuthenticator
{
    public interface IGoogleAuthenticatorProvider
    {
        GoogleAuthenticatorSetupInfos GetUserSetupInfos(string email);
        bool ValidatePin(string secretKey, string pin);
    }
}
