using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage4rest2023.Models.DTOs
{
    public class CheckBoxDTO
    {
        public int DocumentId { get; set; }
        public bool IsArchived { get; set; }
    }
}