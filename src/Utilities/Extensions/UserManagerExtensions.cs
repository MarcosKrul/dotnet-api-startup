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
            string email
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

        public static bool IsPasswordInHistory(
            this UserManager<AppUser> userManager,
            AppUser user,
            string newPassword,
            IEnumerable<string> passwordHistory
        )
        {
            foreach (var oldPasswordHash in passwordHistory)
            {
                var verificationResult = userManager.PasswordHasher.VerifyHashedPassword(
                    user,
                    oldPasswordHash,
                    newPassword
                );

                if (verificationResult == PasswordVerificationResult.Success)
                    return true;
            }

            return false;
        }
    }
}
