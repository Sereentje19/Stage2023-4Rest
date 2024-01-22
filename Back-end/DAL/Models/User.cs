using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Het email mag niet meer dan {1} tekens bevatten.")]
        public string Email { get; set; }
        [StringLength(200, ErrorMessage = "De naam mag niet meer dan {1} tekens bevatten.")]
        public string Name { get; set; }
        [StringLength(250, ErrorMessage = "Er is is fout gegaan bij het aanmaken van het wachtwoord.")]
        public string PasswordHash { get; set; }
        [StringLength(250, ErrorMessage = "Er is is fout gegaan bij het aanmaken van het wachtwoord.")]
        public string PasswordSalt { get; set; }
    }
}
