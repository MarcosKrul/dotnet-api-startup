using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Common;
using TucaAPI.Dtos.Account;
using TucaAPI.Interfaces;
using TucaAPI.Models;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            this.tokenService = tokenService;
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

                if (!createdUser.Succeeded)
                {
                    return StatusCode(HttpStatus.INTERNAL_ERROR, createdUser.Errors);
                }

                var roleResults = await this.userManager.AddToRoleAsync(appUser, "User");

                if (!roleResults.Succeeded)
                {
                    return StatusCode(HttpStatus.INTERNAL_ERROR, roleResults.Errors);
                }

                return Ok(new NewUserDto
                {
                    UserName = appUser.UserName ?? "",
                    Email = appUser.Email ?? "",
                    Token = this.tokenService.Create(appUser)
                });
            }
            catch (Exception exception)
            {
                return StatusCode(HttpStatus.INTERNAL_ERROR, exception);
            }
        }
    }
}