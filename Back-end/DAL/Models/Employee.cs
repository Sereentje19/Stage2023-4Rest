using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool IsArchived { get; set; }
    }
}
