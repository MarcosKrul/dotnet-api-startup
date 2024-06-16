using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Attributes;
using TucaAPI.Dtos.Account;
using TucaAPI.Models;
using TucaAPI.Providers;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Extensions;
using TucaAPI.src.Providers;
using TucaAPI.src.Services.Account;
using TucaAPI.Src.Services.Account;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenProvider tokenProvider;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMailSenderProvider mailProvider;
        private readonly ITemplateRenderingProvider templateRenderingProvider;
        private readonly IServiceProvider serviceProvider;

        public AccountController(
            UserManager<AppUser> userManager,
            ITokenProvider tokenProvider,
            SignInManager<AppUser> signInManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider,
            IServiceProvider serviceProvider
        )
        {
            this.tokenProvider = tokenProvider;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailProvider = mailProvider;
            this.templateRenderingProvider = templateRenderingProvider;
            this.serviceProvider = serviceProvider;
        }

        private async Task<IActionResult> GetAuthenticatedUserAction(AppUser user)
        {
            return Ok(
                new SuccessApiResponse<AuthenticatedUserDto>
                {
                    Content = new AuthenticatedUserDto
                    {
                        UserName = user.UserName.GetNonNullable(),
                        Email = user.Email.GetNonNullable(),
                        Token = await this.tokenProvider.CreateAsync(user)
                    }
                }
            );
        }

        [HttpPost]
        [Route("register")]
        [ValidateModelState]
        public async Task<IActionResult> Register([FromBody] RegisterDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<RegisterAccountService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("confirm")]
        [ValidateModelState]
        public async Task<IActionResult> Confirm([FromBody] ConfirmDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ConfirmAccountService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("login")]
        [ValidateModelState]
        public async Task<IActionResult> Login([FromBody] LoginDto data)
        {
            var unauthorizedError = Unauthorized(
                new ErrorApiResponse(MessageKey.INVALID_CREDENTIALS)
            );

            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                return unauthorizedError;

            var result = await this.signInManager.CheckPasswordSignInAsync(
                hasUser,
                data.Password.GetNonNullable(),
                true
            );

            if (result.IsLockedOut)
                return Unauthorized(new ErrorApiResponse(MessageKey.ACCOUNT_LOCKED));

            if (!result.Succeeded)
                return unauthorizedError;

            return await this.GetAuthenticatedUserAction(hasUser);
        }

        [HttpPost]
        [Route("forgot")]
        [ValidateModelState]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto data)
        {
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                return Unauthorized(new ErrorApiResponse(MessageKey.INVALID_CREDENTIALS));

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

            return Ok(new ApiResponse { Success = true });
        }

        [HttpPost]
        [Route("reset")]
        [ValidateModelState]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto data)
        {
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i =>
                i.Email == data.Email
            );

            if (hasUser is null)
                return Unauthorized(new ErrorApiResponse(MessageKey.INVALID_CREDENTIALS));

            var result = await this.userManager.ResetPasswordAsync(
                hasUser,
                data.Token,
                data.Password
            );

            if (!result.Succeeded)
                return BadRequest(new SuccessApiResponse<IdentityResult> { Content = result });

            return Ok(new ApiResponse { Success = true });
        }
    }
}
