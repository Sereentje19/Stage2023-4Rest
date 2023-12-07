using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsArchived { get; set; }
    }
}
