using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Attributes;
using TucaAPI.Dtos.Account;
using TucaAPI.Models;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;
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
        private readonly IServiceProvider serviceProvider;

        public AccountController(
            UserManager<AppUser> userManager,
            IMailSenderProvider mailProvider,
            ITemplateRenderingProvider templateRenderingProvider,
            IServiceProvider serviceProvider
        )
        {
            this.userManager = userManager;
            this.serviceProvider = serviceProvider;
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
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<LoginService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("forgot")]
        [ValidateModelState]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ForgotPasswordService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
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
