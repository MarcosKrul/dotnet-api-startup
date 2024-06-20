using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Models;

namespace TucaAPI.src.Utilities.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindNonNullableUserAsync(
            this UserManager<AppUser> userManager,
            string email = MessageKey.USER_NOT_FOUND
        )
        {
            var hasUser = await userManager.FindByEmailAsync(email);

            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.USER_NOT_FOUND
                );

            return hasUser;
        }

        public static async Task<AppUser> FindNonNullableUserAsync(
            this UserManager<AppUser> userManager,
            string email,
            string errorKey
        )
        {
            var hasUser = await userManager.FindByEmailAsync(email);

            if (hasUser is null)
                throw new AppException(StatusCodes.Status401Unauthorized, errorKey);

            return hasUser;
        }
    }
}
