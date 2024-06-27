using Microsoft.AspNetCore.Identity;

namespace TucaAPI.src.Models
{
    public class AppUser : IdentityUser
    {
        public string? TwoFactorSecret { get; set; }
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
