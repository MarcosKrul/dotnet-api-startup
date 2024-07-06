using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Dtos.TwoFactorAuthentication;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Utilities.Common;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.TwoFactorAuthentication
{
    public class RequestDisableGoogleAuthenticator2FAService
        : IService<RequestDisableGoogleAuthenticator2FARequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMailSenderProvider mailProvider;
        private readonly ITemplateRenderingProvider templateRenderingProvider;

        public RequestDisableGoogleAuthenticator2FAService(
            UserManager<AppUser> userManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider
        )
        {
            this.userManager = userManager;
            this.mailProvider = mailProvider;
            this.templateRenderingProvider = templateRenderingProvider;
        }

        public async Task<ApiResponse> ExecuteAsync(
            RequestDisableGoogleAuthenticator2FARequestDto data
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

            var token = await this.userManager.GenerateUserTokenAsync(
                user,
                "Default",
                "Disable2FA"
            );
            var link = $"{data.Url}?token={token}";
            var email = user.Email.GetNonNullable();
            var userName = user.UserName.GetNonNullable();

            var templateWriter = this.templateRenderingProvider.Render(
                Path.Combine("Templates", "Mail", "RequestDisable2FA", "index.hbs"),
                new { userName, link }
            );

            await this.mailProvider.SendHtmlAsync(
                new BaseHtmlMailData
                {
                    EmailToId = email,
                    EmailToName = userName,
                    EmailSubject = EmailSubject.REQUEST_DISABLED_2FA,
                    TemplateWriter = templateWriter,
                    EmailBody = $"{Messages.MAIL_DISABLE_2FA}: {link}"
                }
            );

            return new ApiResponse { Success = true };
        }
    }
}
