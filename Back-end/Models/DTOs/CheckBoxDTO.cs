using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Models.DTOs
{
    public class CheckBoxDTO
    {
        public int DocumentId { get; set; }
        public bool IsArchived { get; set; }
    }
}