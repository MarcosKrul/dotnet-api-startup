using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;

namespace TucaAPI.src.Utilities.Extensions
{
    public static class UserAuthenticatedInfosExtensions
    {
        public static void AggregateUser(
            this UserAuthenticatedInfos input,
            System.Security.Claims.ClaimsPrincipal user
        )
        {
            input.User = user;
        }

        public static System.Security.Claims.ClaimsPrincipal GetNonNullableUser(
            this UserAuthenticatedInfos input
        )
        {
            if (input.User is null)
                throw new AppException(
                    StatusCodes.Status500InternalServerError,
                    MessageKey.USER_NOT_FOUND
                );

            return input.User;
        }
    }
}
