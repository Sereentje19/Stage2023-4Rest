using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Back_end.Models
{
    public class Documents
    {
        [Key]
        public int DocumentId { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
        public Customer? Customer { get; set; }
        public User? User { get; set; }
        public Type Type { get; set; }
    }
}