using System.ComponentModel.DataAnnotations;

namespace Stage4rest2023.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}