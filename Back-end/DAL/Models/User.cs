using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
    }
}
