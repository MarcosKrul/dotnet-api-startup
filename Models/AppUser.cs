using Microsoft.AspNetCore.Identity;

namespace TucaAPI.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portifolio> Portifolios { get; set; } = new List<Portifolio>();
    }
}