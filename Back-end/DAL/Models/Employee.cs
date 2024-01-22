using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [StringLength(255, ErrorMessage = "De Email mag niet meer dan {1} tekens bevatten.")]
        public string Email { get; set; }
        [StringLength(200, ErrorMessage = "De naam mag niet meer dan {1} tekens bevatten.")]
        public string Name { get; set; }
        public bool IsArchived { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<LoanHistory> LoanHistory { get; set; }
    }
}
