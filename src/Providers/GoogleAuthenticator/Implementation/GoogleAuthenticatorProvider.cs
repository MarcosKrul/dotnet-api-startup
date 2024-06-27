using Google.Authenticator;
using TucaAPI.src.Dtos.TwoFactorAuthentication;

namespace TucaAPI.src.Providers.GoogleAuthenticator.Implementation
{
    public class GoogleAuthenticatorProvider : IGoogleAuthenticatorProvider
    {
        private readonly TwoFactorAuthenticator twoFactorAuthenticator;

        public GoogleAuthenticatorProvider()
        {
            this.twoFactorAuthenticator = new TwoFactorAuthenticator();
        }

        public GoogleAuthenticatorSetupInfos GetUserSetupInfos(string email)
        {
            var token = Guid.NewGuid().ToString().Replace("-", "")[..10];
            var setupInfo = this.twoFactorAuthenticator.GenerateSetupCode(
                "TUCA API",
                email,
                token,
                false
            );

            return new GoogleAuthenticatorSetupInfos
            {
                QrCodeImageUrl = setupInfo.QrCodeSetupImageUrl,
                Token = token
            };
        }

        public bool ValidatePin(string secretKey, string pin)
        {
            return this.twoFactorAuthenticator.ValidateTwoFactorPIN(secretKey, pin);
        }
    }
}
