using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Utilities.Common;

namespace TucaAPI.src.Services.Account
{
    public class RegisterAccountService : IService<RegisterRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMailSenderProvider mailProvider;
        private readonly ITemplateRenderingProvider templateRenderingProvider;

        public RegisterAccountService(
            UserManager<AppUser> userManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider
        )
        {
            this.userManager = userManager;
            this.mailProvider = mailProvider;
            this.templateRenderingProvider = templateRenderingProvider;
        }

        public async Task<ApiResponse> ExecuteAsync(RegisterRequestDto data)
        {
            var appUser = new AppUser { UserName = data.Username, Email = data.Email };

            var createdUser = await this.userManager.CreateAsync(
                appUser,
                data.Password.GetNonNullable()
            );

            if (!createdUser.Succeeded)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    createdUser.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            var roleResult = await this.userManager.AddToRoleAsync(appUser, PermissionRoles.USER);

            if (!roleResult.Succeeded)
                throw new AppException(
                    StatusCodes.Status400BadRequest,
                    roleResult.Errors.Select(item => item.ToAppErrorDescriptor())
                );

            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var link = $"{data.Url}?token={token}";
            var email = appUser.Email.GetNonNullable();
            var userName = appUser.UserName.GetNonNullable();

            var templateWriter = this.templateRenderingProvider.Render(
                Path.Combine("Templates", "Mail", "ConfirmAccount", "index.hbs"),
                new { userName, link }
            );

            await this.mailProvider.SendHtmlAsync(
                new BaseHtmlMailData
                {
                    EmailToId = email,
                    EmailToName = userName,
                    EmailSubject = EmailSubject.CONFIRM_ACCOUNT,
                    TemplateWriter = templateWriter,
                    EmailBody = $"{Messages.MAIL_CONFIRM_ACCOUNT}: {link}"
                }
            );

            return new ApiResponse { Success = true };
        }
    }
}
