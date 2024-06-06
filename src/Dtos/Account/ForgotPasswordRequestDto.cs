using System.ComponentModel.DataAnnotations;

namespace TucaAPI.src.Dtos.Account
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}