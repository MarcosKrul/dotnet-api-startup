using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
using TucaAPI.Models;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;

        public UserManagementController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpDelete]
        [ValidateModelState]
        [Authorize(Policy = AuthorizationPolicies.ADMIN_ONLY)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var hasUser = await this.userManager.FindByIdAsync(id);
            if (hasUser is null)
                return NotFound(new ErrorApiResponse(MessageKey.USER_NOT_FOUND));

            // delete

            return Ok();
        }
    }
}
