using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    public class Documents
    {
        [Key]
        public int DocumentId { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [Column(TypeName = "nvarchar(24)")]
        public Type Type { get; set; }
    }
}