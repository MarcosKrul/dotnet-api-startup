using System.Secutiry.Claims;

namespace TucaAPI.Extensions 
{
    public class ClaimsExtensions 
    {
        public static string GetEmail(this ClaimsPrincipal user) 
        {
            return user.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }
    }
}