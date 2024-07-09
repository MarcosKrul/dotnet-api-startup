using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Services.Account;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public AccountController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpPost]
        [Route("register")]
        [ValidateModelState]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto data)
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
        public async Task<IActionResult> Confirm([FromBody] ConfirmRequestDto data)
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<
                    LoginService<LoginRequestDto>
                >();
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
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ResetPasswordService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPatch]
        [Route("password")]
        [ValidateModelState]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                data.AggregateUser(User);
                var service = scope.ServiceProvider.GetRequiredService<UpdatePasswordService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }
    }
}
