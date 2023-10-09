using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public byte[]? File { get; set; }
        public string? FileType { get; set; }
        public DateTime Date { get; set; }
        
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public Type Type { get; set; }
    }
}