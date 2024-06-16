using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;

namespace TucaAPI.src.Services.Account
{
    public class ForgotPasswordService : IService<ForgotPasswordRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMailSenderProvider mailProvider;
        private readonly ITemplateRenderingProvider templateRenderingProvider;

        public ForgotPasswordService(
            UserManager<AppUser> userManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider
        )
        {
            this.userManager = userManager;
            this.mailProvider = mailProvider;
            this.templateRenderingProvider = templateRenderingProvider;
        }

        public async Task<ApiResponse> ExecuteAsync(ForgotPasswordRequestDto data)
        {
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.INVALID_CREDENTIALS
                );

            var token = await this.userManager.GeneratePasswordResetTokenAsync(hasUser);
            var link = $"{data.Url}?token={token}";
            var email = hasUser.Email.GetNonNullable();
            var userName = hasUser.UserName.GetNonNullable();

            var templateWriter = this.templateRenderingProvider.Render(
                Path.Combine("Templates", "Mail", "ForgotPassword", "index.hbs"),
                new { userName, link }
            );

            await this.mailProvider.SendHtmlAsync(
                new BaseHtmlMailData
                {
                    EmailToId = email,
                    EmailToName = userName,
                    EmailSubject = email,
                    TemplateWriter = templateWriter,
                    EmailBody = $"{Messages.MAIL_RESET_PASSWORD}: {link}"
                }
            );

            return new ApiResponse { Success = true };
        }
    }
}
