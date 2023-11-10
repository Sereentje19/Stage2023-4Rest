using System.ComponentModel.DataAnnotations;

namespace Stage4rest2023.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public bool IsArchived { get; set; }
    }
}