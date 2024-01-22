using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class PasswordResetCode
{
    [Key]
    public int ResetCodeId { get; set; }
    [ForeignKey("UserId")]
    [Required]
    public int UserId { get; set; } 
    [Required]
    [StringLength(10, ErrorMessage = "Er is is fout gegaan bij het versturen van de code, probeer het later nog eens.")]
    public string Code { get; set; }
    [Required]
    public DateTime ExpirationTime { get; set; }
}
