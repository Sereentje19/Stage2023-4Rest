using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PL.Models;

public class PasswordResetCode
{
    [Key]
    public int ResetCodeId { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; } 
    public string Code { get; set; }
    public DateTime ExpirationTime { get; set; }
}
