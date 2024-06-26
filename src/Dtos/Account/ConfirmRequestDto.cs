using System.ComponentModel.DataAnnotations;

namespace TucaAPI.src.Dtos.Account
{
    public class ConfirmRequestDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
