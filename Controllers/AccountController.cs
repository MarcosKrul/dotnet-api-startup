using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Dtos.Account;
using TucaAPI.Models;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
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

                if (createdUser.Succeeded)
                {
                    var roleResults = await this.userManager.AddToRoleAsync(appUser, "User");
                    if (roleResults.Succeeded) return Ok();
                    else return StatusCode(500, roleResults.Errors);
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception);
            }
        }
    }
}