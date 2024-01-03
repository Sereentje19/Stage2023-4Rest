using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<LoanHistory> LoanHistory { get; set; }
    }
}
