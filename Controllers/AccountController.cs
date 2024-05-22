using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Attributes;
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
        private readonly SignInManager<AppUser> signInManager;

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
            SignInManager<AppUser> signInManager
        )
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            var unauthorizedError = Unauthorized("Invalid credentials");

            var hasUser = await this.userManager.Users.FirstOrDefaultAsync(i => i.Email == data.Email);

            if (hasUser == null) return unauthorizedError;

            var result = await this.signInManager.CheckPasswordSignInAsync(hasUser, data.Password ?? "", false);

            if (!result.Succeeded) return unauthorizedError;

            return this.GetAuthenticatedUserAction(hasUser);
        }
    }
}