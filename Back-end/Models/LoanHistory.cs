using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    public class LoanHistory
    {

        [Key]
        public int LoanHistoryId { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }

    }
}