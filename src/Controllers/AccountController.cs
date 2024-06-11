using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Attributes;
using TucaAPI.Common;
using TucaAPI.Dtos.Account;
using TucaAPI.Interfaces;
using TucaAPI.Models;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Interfaces;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMailSenderService mailService;

        private IActionResult GetAuthenticatedUserAction(AppUser user)
        {
            return Ok(new AuthenticatedUserDto
            {
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                Token = this.tokenService.Create(user)
            });
        }

        public AccountController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            SignInManager<AppUser> signInManager,
            IMailSenderService mailService
        )
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("register")]
        [ValidateModelState]
        public async Task<IActionResult> Register([FromBody] RegisterDto data)
        {
            if (string.IsNullOrEmpty(data.Password)) return BadRequest();

            try
            {
                var appUser = new AppUser
                {
                    UserName = data.Username,
                    Email = data.Email
                };

                var createdUser = await this.userManager.CreateAsync(appUser, data.Password);

                if (!createdUser.Succeeded) return StatusCode(HttpStatus.INTERNAL_ERROR, createdUser.Errors);

                var roleResults = await this.userManager.AddToRoleAsync(appUser, "User");

                if (!roleResults.Succeeded) return StatusCode(HttpStatus.INTERNAL_ERROR, roleResults.Errors);

                return this.GetAuthenticatedUserAction(appUser);
            }
            catch (Exception exception)
            {
                return StatusCode(HttpStatus.INTERNAL_ERROR, exception);
            }
        }

        [HttpPost]
        [Route("login")]
        [ValidateModelState]
        public async Task<IActionResult> Login([FromBody] LoginDto data)
        {
            var unauthorizedError = Unauthorized(Messages.INVALID_CREDENTIALS);

            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i => i.Email == data.Email);

            if (hasUser is null) return unauthorizedError;

            var result = await this.signInManager.CheckPasswordSignInAsync(hasUser, data.Password ?? "", true);

            if (!result.Succeeded) return unauthorizedError;

            return this.GetAuthenticatedUserAction(hasUser);
        }

        [HttpPost]
        [Route("forgot")]
        [ValidateModelState]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto data)
        {
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i => i.Email == data.Email);

            if (hasUser is null) return Unauthorized(Messages.INVALID_CREDENTIALS);

            var token = await this.userManager.GeneratePasswordResetTokenAsync(hasUser);

            await this.mailService.SendAsync(new BaseMailData
            {
                EmailToId = hasUser.Email ?? "",
                EmailToName = hasUser.UserName ?? "",
                EmailSubject = hasUser.Email ?? "",
                EmailBody = String.Format("{0}: {1}?token={2}", Messages.MAIL_RESET_PASSWORD, data.Url, token)
            });

            return Ok();
        }

        [HttpPost]
        [Route("reset")]
        [ValidateModelState]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto data)
        {
            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i => i.Email == data.Email);

            if (hasUser is null) return Unauthorized(Messages.INVALID_CREDENTIALS);

            var result = await this.userManager.ResetPasswordAsync(hasUser, data.Token, data.Password);

            if (!result.Succeeded) return BadRequest(result);

            return Ok();
        }
    }
}