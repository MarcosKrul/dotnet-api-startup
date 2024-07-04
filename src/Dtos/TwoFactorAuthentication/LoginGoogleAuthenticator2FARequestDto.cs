using System.ComponentModel.DataAnnotations;
using TucaAPI.src.Dtos.Account;

namespace TucaAPI.src.Dtos.TwoFactorAuthentication
{
    public class LoginGoogleAuthenticator2FARequestDto : LoginRequestDto
    {
        [Required]
        public string? Pin { get; set; }
    }
}
