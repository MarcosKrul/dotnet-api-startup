using System.ComponentModel.DataAnnotations.Schema;

namespace TucaAPI.src.Models
{
    [Table("PasswordHistory")]
    public class PasswordHistory
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Password { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public AppUser AppUser { get; set; }
    }
}
