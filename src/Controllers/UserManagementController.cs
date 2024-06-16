using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Common;
using TucaAPI.src.Services.UserManagement;

namespace TucaAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public UserManagementController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpDelete]
        [ValidateModelState]
        [Authorize(Policy = AuthorizationPolicies.ADMIN_ONLY)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<DeleteUserService>();
                var result = await service.ExecuteAsync(id);
                return Ok(result);
            }
        }
    }
}
