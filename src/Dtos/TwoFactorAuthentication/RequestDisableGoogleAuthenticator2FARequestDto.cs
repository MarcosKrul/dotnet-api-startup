using System.ComponentModel.DataAnnotations;
using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.TwoFactorAuthentication
{
    public class RequestDisableGoogleAuthenticator2FARequestDto : UserAuthenticatedInfos
    {
        [Required]
        public string? Url { get; set; }
    }
}
