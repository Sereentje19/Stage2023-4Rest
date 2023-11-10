using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage4rest2023.Models.DTOs
{
    public class CheckBoxRequest
    {
        public int DocumentId { get; }
        public bool IsArchived { get; }
    }
}