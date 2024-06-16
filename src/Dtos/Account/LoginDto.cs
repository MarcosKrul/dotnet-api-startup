using System.ComponentModel.DataAnnotations;

namespace TucaAPI.src.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
