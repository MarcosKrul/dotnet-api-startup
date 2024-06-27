using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.TwoFactorAuthentication;
using TucaAPI.src.Services.TwoFactorAuthentication;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwoFactorAuthenticationController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public TwoFactorAuthenticationController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpPost("enableGoogleAuthenticator2FA")]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> EnableGoogleAuthenticator2FA()
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var data = new UserAuthenticatedInfos();
                data.AggregateUser(User);
                var service =
                    scope.ServiceProvider.GetRequiredService<EnableGoogleAuthenticator2FAService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("loginGoogleAuthenticator2FA")]
        [ValidateModelState]
        public async Task<IActionResult> Login(
            [FromBody] LoginGoogleAuthenticator2FARequestDto data
        )
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider.GetRequiredService<LoginGoogleAuthenticator2FAService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }
    }
}
