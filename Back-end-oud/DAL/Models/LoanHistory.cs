using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class LoanHistory
    {
        [Key]
        public int LoanHistoryId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
