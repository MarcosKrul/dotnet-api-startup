using System.ComponentModel.DataAnnotations;
using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.TwoFactorAuthentication
{
    public class ConfirmDisableGoogleAuthenticator2FARequestDto : UserAuthenticatedInfos
    {
        [Required]
        public string? Token { get; set; }

        [Required]
        public string? Pin { get; set; }
    }
}
