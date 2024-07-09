using System.ComponentModel.DataAnnotations;
using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Account
{
    public class UpdatePasswordRequestDto : UserAuthenticatedInfos
    {
        [Required]
        public string? NewPassword { get; set; }

        [Required]
        public string? OldPassword { get; set; }
    }
}
