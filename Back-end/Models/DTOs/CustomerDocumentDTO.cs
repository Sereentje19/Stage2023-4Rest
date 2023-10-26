using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Models.DTOs
{
    public class CustomerDocumentDTO
    {
        public int CustomerId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public int DocumentId { get; set; }
    }
}