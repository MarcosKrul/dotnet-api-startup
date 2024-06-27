using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Providers.GoogleAuthenticator;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.TwoFactorAuthentication
{
    public class EnableGoogleAuthenticator2FAService
        : IService<UserAuthenticatedInfos, SuccessApiResponse<string>>
    {
        private readonly IGoogleAuthenticatorProvider googleAuthenticatorProvider;
        private readonly UserManager<AppUser> userManager;
        private readonly IMailSenderProvider mailProvider;
        private readonly ITemplateRenderingProvider templateRenderingProvider;

        public EnableGoogleAuthenticator2FAService(
            IGoogleAuthenticatorProvider googleAuthenticatorProvider,
            UserManager<AppUser> userManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider
        )
        {
            this.googleAuthenticatorProvider = googleAuthenticatorProvider;
            this.userManager = userManager;
            this.mailProvider = mailProvider;
            this.templateRenderingProvider = templateRenderingProvider;
        }

        public async Task<SuccessApiResponse<string>> ExecuteAsync(UserAuthenticatedInfos data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            if (user.TwoFactorEnabled)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    MessageKey.TWO_FACTOR_ALREADY_ENABLED
                );

            var email = user.Email.GetNonNullable();

            var infos = this.googleAuthenticatorProvider.GetUserSetupInfos(email);

            user.TwoFactorSecret = infos.Token;
            user.TwoFactorEnabled = true;
            var result = await this.userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new AppException(
                    StatusCodes.Status500InternalServerError,
                    MessageKey.ERROR_UPDATE_USER
                );

            var userName = user.UserName.GetNonNullable();

            var templateWriter = this.templateRenderingProvider.Render(
                Path.Combine("Templates", "Mail", "Verify2FA", "index.hbs"),
                new { userName, qrCodeBase64 = infos.QrCodeImageUrl }
            );

            await this.mailProvider.SendHtmlAsync(
                new BaseHtmlMailData
                {
                    EmailToId = email,
                    EmailToName = userName,
                    EmailSubject = email,
                    TemplateWriter = templateWriter,
                    EmailBody = $"{Messages.MAIL_VERIFY_2FA}"
                }
            );

            return new SuccessApiResponse<string> { Content = infos.QrCodeImageUrl };
        }
    }
}
