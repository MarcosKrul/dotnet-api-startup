using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Providers;
using TucaAPI.src.Repositories;
using TucaAPI.src.Utilities.Common;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Account
{
    public class LoginService<T> : IService<T, SuccessApiResponse<AuthenticatedUserDto>>
        where T : LoginRequestDto
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenProvider tokenProvider;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMailSenderProvider mailProvider;
        private readonly ITemplateRenderingProvider templateRenderingProvider;
        private readonly IPasswordHistoryRepository passwordHistoryRepository;

        public LoginService(
            UserManager<AppUser> userManager,
            ITokenProvider tokenProvider,
            SignInManager<AppUser> signInManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider,
            IPasswordHistoryRepository passwordHistoryRepository
        )
        {
            this.tokenProvider = tokenProvider;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailProvider = mailProvider;
            this.templateRenderingProvider = templateRenderingProvider;
            this.passwordHistoryRepository = passwordHistoryRepository;
        }

        protected virtual void VerifyTwoFactorAuthentication(AppUser user, T data)
        {
            if (user.TwoFactorEnabled)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.TWO_FACTOR_REQUIRED
                );
        }

        public async Task<SuccessApiResponse<AuthenticatedUserDto>> ExecuteAsync(T data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.Email.GetNonNullable(),
                MessageKey.INVALID_CREDENTIALS
            );

            this.VerifyTwoFactorAuthentication(user, data);

            var result = await this.signInManager.CheckPasswordSignInAsync(
                user,
                data.Password.GetNonNullable(),
                true
            );

            if (result.IsLockedOut)
            {
                var email = user.Email.GetNonNullable();
                var userName = user.UserName.GetNonNullable();

                var templateWriter = this.templateRenderingProvider.Render(
                    Path.Combine("Templates", "Mail", "AccountLockedOut", "index.hbs"),
                    new { userName }
                );

                await this.mailProvider.SendHtmlAsync(
                    new BaseHtmlMailData
                    {
                        EmailToId = email,
                        EmailToName = userName,
                        EmailSubject = EmailSubject.ACCOUNT_LOCKED_OUT,
                        TemplateWriter = templateWriter,
                        EmailBody = Messages.ACCOUNT_LOCKED_OUT
                    }
                );

                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.ACCOUNT_LOCKED
                );
            }

            if (!result.Succeeded)
            {
                var hasRecentPasswordChange =
                    await this.passwordHistoryRepository.GetMostRecentPasswordHistoryAsync(
                        user.Id,
                        DateTime.Now.Add(Constants.TIME_TO_NOTIFY_THAT_PASSWORD_HAS_BEEN_CHANGED)
                    );

                if (hasRecentPasswordChange is not null)
                    throw new AppException(
                        StatusCodes.Status401Unauthorized,
                        [
                            new AppErrorDescriptor
                            {
                                Key = MessageKey.INVALID_CREDENTIALS,
                                Description =
                                    $"{Messages.PASSWORD_CHANGED} {hasRecentPasswordChange.UpdatedOn.GetReadableString()}"
                            }
                        ]
                    );

                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.INVALID_CREDENTIALS
                );
            }

            var token = await this.tokenProvider.CreateAsync(user);

            return new SuccessApiResponse<AuthenticatedUserDto>
            {
                Content = new AuthenticatedUserDto
                {
                    UserName = user.UserName.GetNonNullable(),
                    Email = user.Email.GetNonNullable(),
                    Token = token
                }
            };
        }
    }
}
