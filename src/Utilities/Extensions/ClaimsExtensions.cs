using System.Security.Claims;

namespace TucaAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetUsername(this ClaimsPrincipal user)
        {
            return user
                .Claims.SingleOrDefault(i =>
                    i.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
                )
                ?.Value;
        }

        public static string? GetEmail(this ClaimsPrincipal user)
        {
            return user
                .Claims.SingleOrDefault(i =>
                    i.Type.Equals(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                    )
                )
                ?.Value;
        }
    }
}
