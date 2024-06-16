using System.ComponentModel.DataAnnotations;

namespace TucaAPI.src.Dtos.Account
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
